using System;
using System.Net.Http;
using System.Threading.Tasks;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    /// <summary>
    /// Service for processing SSL Labs Requests
    /// Note: SSL Labs requests can take some time if the report has not recently been run.
    /// </summary>
    public class SslLabs : Service<SslLabsModel>
    {
        private HttpClient _client;

        private int _backOffMs = 500;


        public SslLabs(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// SSL Labs can take some time to process the request.
        ///
        /// This method polls for completion result. with a dynamic back of
        /// </summary>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override async Task<IServiceResult<SslLabsModel>> DoLookUp(string address, AddressType type)
        {
            var url = $"https://api.ssllabs.com/api/v3/analyze?host={address}";

            var res = await _client.GetAsync(url);
            res.EnsureSuccessStatusCode();

            var body = ParseBody(await res.Content.ReadAsStringAsync());

            if (body.Status.Equals("READY", StringComparison.OrdinalIgnoreCase))
                return new ServiceResult<SslLabsModel>
                {
                    Data = body,
                    Status = ServiceStatus.Ok,
                    Type = ServiceType.SslLabs
                };


            await Task.Delay(_backOffMs);
            SetbackOff();

            return await DoLookUp(address, type);

        }

        /// <summary>
        /// increase back off delay by 2 until 30 seconds then, set back to 1 second
        /// </summary>
        private void SetbackOff()
        {
            _backOffMs *= 2;

            if (_backOffMs > 30_000)
                _backOffMs = 1000;
        }
    }
}