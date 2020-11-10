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
using Microsoft.Extensions.Logging;

namespace IPdLookUp.Core.Models
{
    /// <summary>
    /// Main class for sending request to pool of workers
    /// </summary>
    public static class WorkerHelper
    {
        private static HttpClient _client = new HttpClient();

        /// <summary>
        /// Used for when a request has an invalid type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static AppResult HandleInvalidResult(ServiceType type) => SetItemIntoResult(new ServiceResult<string>
        {
            Status = ServiceStatus.Bad,
            Type = type,
            ErrorMessage = $"Invalid service type passed in. Expected {type} to be of {nameof(ServiceType)}",
            WorkerId = Environment.MachineName,
        });

        /// <summary>
        /// Send list of services to be processed.
        /// </summary>
        /// <param name="workerAddress">Address to reach the worker api. with api path. i.e. api.com/api/worker</param>
        /// <param name="addressToCheck">the address (domain/IPv4) to run services on</param>
        /// <param name="types">services to run again addressToCheck</param>
        /// <returns></returns>
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

        /// <summary>
        /// Actual method that fires off request to the worker api
        /// </summary>
        /// <param name="url">formatted url for the worker api</param>
        /// <param name="type">service type to run</param>
        /// <typeparam name="TModel">Entity that will be inside the Data property of IServiceResponse</typeparam>
        /// <returns></returns>
        private static async Task<AppResult> Send<TModel>(string url, ServiceType type)
        {
            try
            {
                var res = await _client.GetAsync(url);
                res.EnsureSuccessStatusCode();

                var content = await res.Content.ReadAsStringAsync();
                var body = JsonSerializer.Deserialize<ServiceResult<TModel>>(content);
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

        /// <summary>
        /// Translate IServiceResult<TModel> into AppResult by setting property related to TModel.
        /// Other properties will be null
        /// See <see cref="MergeItems"/> to combine several AppResults into a single one.
        /// </summary>
        /// <param name="data">Data from worker API</param>
        /// <typeparam name="TModel">Model of the response</typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Take list of AppResults and merge the properties into one AppResult.
        /// Note: Properties will override each other if not null
        /// </summary>
        /// <param name="items"></param>
        /// <returns>Merged AppResult</returns>
        private static AppResult MergeItems(IEnumerable<AppResult> items)
        {
            var clean = new AppResult();
            foreach (var item in items)
            {
                CopyValues(clean, item);
            }

            return clean;
        }

        /// <summary>
        /// Will copy values from source to target. Requires a reference type otherwise will always be null
        /// </summary>
        /// <param name="target">Reference type (i.e. class)</param>
        /// <param name="source">Reference type (i.e. class)</param>
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