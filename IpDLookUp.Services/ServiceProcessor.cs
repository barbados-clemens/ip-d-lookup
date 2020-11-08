using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    public static class ServiceProcessor
    {
        private static HttpClient _client = new HttpClient();

        private static string NormalizeAddress(string address, out AddressType addressType)
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

                res.WorkerId = Environment.MachineName;
                return res;
            }
            catch (Exception e)
            {
                return new ServiceResult<TModel>
                {
                    Status = ServiceStatus.Error,
                    Type = type,
                    ErrorMessage = $"Uncaught Error: {e}",
                    WorkerId = Environment.MachineName,
                };
            }
        }
    }
}