using System.Net.Http;
using System.Threading.Tasks;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    /// <summary>
    /// Service for processing GeoIP Requests
    /// </summary>
    public class GeoIp : Service<GeoIpModel>
    {
        private HttpClient _client;

        public GeoIp(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Look up the Geolocation of an ip or address.
        /// NOTE: the address must not have a protocol with it.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override async Task<IServiceResult<GeoIpModel>> DoLookUp(string address, AddressType type)
        {
            var url = $"https://freegeoip.app/json/{address}";
            var res = await _client.GetAsync(url);

            res.EnsureSuccessStatusCode();

            var body = ParseBody(await res.Content.ReadAsStringAsync());

            return new ServiceResult<GeoIpModel>
            {
                Data = body,
                Status = ServiceStatus.Ok,
                Type = ServiceType.GeoIP,
            };
        }
    }
}