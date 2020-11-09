using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using IPdLookUp.Core.Entities;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;

namespace IPdLookUp.Core.Models
{
    public static class WorkerHelper
    {
        private static HttpClient _client = new HttpClient();

        public static AppResult HandleInvalidResult(ServiceType type) => SetItemIntoResult(new ServiceResult<string>
        {
            Status = ServiceStatus.Bad,
            Type = type,
            ErrorMessage = $"Invalid service type passed in. Expected {type} to be of {nameof(ServiceType)}",
            WorkerId = Environment.MachineName,
        });

        public static async Task<AppResult> SendToWorkers(string workerAddress, string addressToCheck,
            List<ServiceType> types)
        {
            var serviceTasks = types.Select(service =>
            {
                var url = $"{workerAddress}/{addressToCheck}?type={(int) service}";
                return service switch
                {
                    ServiceType.GeoIP => Send<GeoIpModel>(url, service),
                    ServiceType.RDAP => Send<RdapModel>(url, service),
                    ServiceType.ReverseDNS => Send<IPHostEntry>(url, service),
                    ServiceType.SslLabs => Send<SslLabsModel>(url, service),
                    ServiceType.Ping => Send<PingModel>(url, service),
                    _ => Task.Factory.StartNew(() => HandleInvalidResult(service)),
                };
            });

            var items = await Task.WhenAll(serviceTasks);

            var single = MergeItems(items);

            single.Address = addressToCheck;
            single.Services = types;
            return single;
        }

        private static async Task<AppResult> Send<TModel>(string url, ServiceType type)
        {
            try
            {
                var res = await _client.GetAsync(url);

                var body = JsonSerializer.Deserialize<ServiceResult<TModel>>(await res.Content.ReadAsStringAsync());
                return SetItemIntoResult(body);
            }
            catch (Exception e)
            {
                return SetItemIntoResult(new ServiceResult<TModel>
                {
                    Status = ServiceStatus.Error,
                    Type = type,
                    ErrorMessage = e.ToString(),
                });
            }
        }

        private static AppResult SetItemIntoResult<TModel>(IServiceResult<TModel> data)
        {
            var result = new AppResult();
            switch (data.Type)
            {
                case ServiceType.GeoIP:
                    result.GeoIp = (ServiceResult<GeoIpModel>) data;
                    break;
                case ServiceType.RDAP:
                    result.Rdap = (ServiceResult<RdapModel>) data;
                    break;
                case ServiceType.ReverseDNS:
                    result.ReverseDns = (ServiceResult<IPHostEntry>) data;
                    break;
                case ServiceType.SslLabs:
                    result.SslLabs = (ServiceResult<SslLabsModel>) data;
                    break;
                case ServiceType.Ping:
                    result.Ping = (ServiceResult<PingModel>) data;
                    break;
                default:
                    result.ErrorMessage =
                        $"Invalid service type. Expected {data.Type} to be of type {nameof(ServiceType)}";
                    result.FailServices = new[] {data.Type};
                    break;
            }

            return result;
        }

        private static AppResult MergeItems(IEnumerable<AppResult> items)
        {
            var clean = new AppResult();
            foreach (var item in items)
            {
                CopyValues(clean, item);
            }

            return clean;
        }

        private static void CopyValues(AppResult target, AppResult source)
        {
            var appTypes = typeof(AppResult);

            var props = appTypes.GetProperties()
                .Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in props)
            {
                var value = prop.GetValue(source, null);
                if (value != null)
                    prop.SetValue(target, value, null);
            }
        }
    }
}