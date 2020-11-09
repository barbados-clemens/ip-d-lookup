using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    public class PingService : Service<PingModel>
    {
        public override async Task<IServiceResult<PingModel>> DoLookUp(string address, AddressType type)
        {
            try
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
                    return new ServiceResult<PingModel>
                    {
                        Status = ServiceStatus.Error,
                        Type = ServiceType.Ping,
                        ErrorMessage = "Unable to retrieve ping results. Ping reply is null",
                    };

                return new ServiceResult<PingModel>
                {
                    Data = new PingModel
                    {
                        Status = res.Status,
                        Address = res.Address.ToString(),
                        BufferSize = res.Buffer.Length,
                        Options = res.Options,
                        RoundTripTime = res.RoundtripTime,
                    },
                    Status = ServiceStatus.Ok,
                    Type = ServiceType.Ping,
                };
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case FormatException _:
                    case ArgumentNullException _:
                        return new ServiceResult<PingModel>
                        {
                            Type = ServiceType.Ping,
                            Status = ServiceStatus.Bad,
                            ErrorMessage = ex.ToString(),
                        };
                    case SocketException _:
                        return new ServiceResult<PingModel>
                        {
                            Type = ServiceType.Ping,
                            Status = ServiceStatus.Error,
                            ErrorMessage =
                                $@"Socket Exception. This typically means the address wasn't able to be looked up. 
                        Detailed Error: {ex}"
                        };
                    default:
                        throw;
                }
            }
        }
    }
}