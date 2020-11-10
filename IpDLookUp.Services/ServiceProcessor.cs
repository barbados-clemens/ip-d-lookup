using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    /// <summary>
    /// Wrapper around each service that handles address normalization and error handling
    /// </summary>
    public static class ServiceProcessor
    {
        private static HttpClient _client = new HttpClient();

        public static string NormalizeAddress(string address, out AddressType addressType)
        {
            if (Validators.DomainName.IsMatch(address))
            {
                addressType = AddressType.DomainName;
                return Regex.Replace(address, @"^(ht|f)tp(s?)\:\/\/", "");
            }

            if (Validators.IPv4.IsMatch(address))
            {
                addressType = AddressType.Ip;
                return address.Trim();
            }

            throw new ArgumentException(
                $"Expected {nameof(address)} to be a valid domain name or IPv4 address. unable to parse {address} as valid address type");
        }

        public static async Task<IServiceResult<TModel>> Process<TModel>(string address, ServiceType type)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            try
            {
                address = NormalizeAddress(address, out var addressType);

                var res = type switch
                {
                    ServiceType.GeoIP => (IServiceResult<TModel>) await new GeoIp(_client).DoLookUp(address,
                        addressType),
                    ServiceType.RDAP => (IServiceResult<TModel>) await new Rdap(_client).DoLookUp(address, addressType),
                    ServiceType.ReverseDNS => (IServiceResult<TModel>) await new ReverseDns().DoLookUp(address,
                        addressType),
                    ServiceType.SslLabs => (IServiceResult<TModel>) await new SslLabs(_client).DoLookUp(address,
                        addressType),
                    ServiceType.Ping => (IServiceResult<TModel>) await new PingService().DoLookUp(address, addressType),
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };

                sw.Stop();
                res.WorkerId = Environment.MachineName;
                res.ElapsedMs = sw.ElapsedMilliseconds;
                return res;
            }
            catch (Exception e)
            {
                sw.Stop();
                return new ServiceResult<TModel>
                {
                    Status = ServiceStatus.Error,
                    Type = type,
                    ErrorMessage = $"Uncaught Error: {e}",
                    WorkerId = Environment.MachineName,
                    ElapsedMs = sw.ElapsedMilliseconds
                };
            }
        }
    }
}