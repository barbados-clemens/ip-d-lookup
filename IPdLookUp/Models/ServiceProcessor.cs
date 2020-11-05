using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IPdLookUp.Entities;
using IPdLookUp.Types;
using IPdLookUp.Validators;

namespace IPdLookUp.Models
{
    public class ServiceProcessor
    {
        private static HttpClient _client = new HttpClient();

        private static string NormalizeAddress(string address, out AddressType addressType)
        {
            if (ValidAddressAttribute.DomainName.IsMatch(address))
            {
                addressType = AddressType.DomainName;
                return Regex.Replace(address, @"^(ht|f)tp(s?)\:\/\/", "");
            }

            if (ValidAddressAttribute.IPv4.IsMatch(address))
            {
                addressType = AddressType.Ip;
                return address.Trim();
            }

            throw new ArgumentException(
                $"Expected {nameof(address)} to be a valid domain name or IPv4 address. unable to parse {address} as valid address type");
        }

        public static async Task<LookUpResult> Process(string address, LookUpService type)
        {
            try
            {
                address = NormalizeAddress(address, out var addressType);

                switch (type)
                {
                    case LookUpService.GeoIP:
                        return await doGeoLookUp(address);
                    case LookUpService.RDAP:
                        return await doRdapLookUp(address, addressType);
                    case LookUpService.ReverseDNS:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new LookUpResult
                {
                    Data = null,
                    Status = LookUpStatus.Error,
                    Type = type,
                    ErrorMessage = e.Message,
                };
            }

            return new LookUpResult();
        }

        /// <summary>
        /// Look up the Geolocation of an ip or address.
        /// NOTE: the address must not have a protocol with it.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private static async Task<LookUpResult> doGeoLookUp(string address)
        {
            var url = $"https://freegeoip.app/json/{address}";
            var res = await _client.GetAsync(url);

            res.EnsureSuccessStatusCode();

            var body = ParseBody<GeoIp>(await res.Content.ReadAsStringAsync());

            return new LookUpResult
            {
                Data = body,
                Status = LookUpStatus.Ok,
                Type = LookUpService.GeoIP,
            };
        }

        private static async Task<LookUpResult> doRdapLookUp(string address, AddressType type)
        {
            var url = new StringBuilder("https://rdap.verisign.com/com/v1/");

            switch (type)
            {
                case AddressType.DomainName:
                    url.Append("domain/");
                    break;
                case AddressType.Ip:
                    url.Append("ip/");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            url.Append(address);
            
            var res = await _client.GetAsync(url.ToString());
            res.EnsureSuccessStatusCode();

            var body = ParseBody<Rdap>(await res.Content.ReadAsStringAsync());

            return new LookUpResult
            {
                Data = body,
                Status = LookUpStatus.Ok,
                Type = LookUpService.RDAP,
            };
        }


        private static TEntity ParseBody<TEntity>(string body)
        {
            return JsonSerializer.Deserialize<TEntity>(body);
        }
    }
}