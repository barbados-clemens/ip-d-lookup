using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    public class Rdap : Service<RdapModel>
    {
        private HttpClient _client;

        public Rdap(HttpClient client)
        {
            _client = client;
        }

        public override async Task<IServiceResult<RdapModel>> DoLookUp(string address, AddressType type)
        {
            // use handy redirect service; therefore, not having to use tld matching
            // https://openrdap.org/api
            var url = type switch
            {
                AddressType.DomainName => $"https://www.rdap.net/domain/{address}",
                AddressType.Ip => $"https://www.rdap.net/ip/{address}",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            var res = await _client.GetAsync(url);
            res.EnsureSuccessStatusCode();

            var body = ParseBody(await res.Content.ReadAsStringAsync());

            return new ServiceResult<RdapModel>
            {
                Data = body,
                Status = ServiceStatus.Ok,
                Type = ServiceType.RDAP,
            };
        }
    }
}