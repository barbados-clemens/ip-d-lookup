using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    public class PingService : Service
    {
        public override async Task<IServiceResult> DoLookUp(string address, AddressType type)
        {
            var p = new Ping();
            var opts = new PingOptions
            {
                Ttl = 32,
                DontFragment = true,
            };
            var buffer = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"); // 32 byte payload
            var res = await p.SendPingAsync(address, 30, buffer, opts);

            if (res == null)
                return new ServiceResult
                {
                    Data = null,
                    Status = ServiceStatus.Error,
                    Type = ServiceType.Ping,
                    ErrorMessage = "Unable to retrive ping results. Ping reply is null",
                };

            return new ServiceResult
            {
                Data = new PingModel
                {
                    Status = res.Status,
                    Address = res.Address,
                    BufferSize = res.Buffer.Length,
                    Options = res.Options,
                    RoundTripTime = res.RoundtripTime,
                },
                Status = ServiceStatus.Ok,
                Type = ServiceType.Ping,
            };
        }
    }
}