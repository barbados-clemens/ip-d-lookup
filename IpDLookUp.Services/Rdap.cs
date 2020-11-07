using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    public class Rdap : Service
    {
        private HttpClient _client;

        public Rdap(HttpClient client)
        {
            _client = client;
        }
        public override async Task<IServiceResult> DoLookUp(string address, AddressType type)
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

            var body = ParseBody<RdapModel>(await res.Content.ReadAsStringAsync());

            return new ServiceResult
            {
                Data = body,
                Status = ServiceStatus.Ok,
                Type = ServiceType.RDAP,
            };

        }
    }
}