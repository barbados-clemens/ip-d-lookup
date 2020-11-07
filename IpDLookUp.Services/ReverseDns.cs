using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    public class ReverseDns : Service
    {
        public override async Task<IServiceResult> DoLookUp(string address, AddressType type)
        {
            try
            {
                var hostIpAddress = IPAddress.Parse(address);
                var hostInfo = await Dns.GetHostEntryAsync(hostIpAddress);

                return new ServiceResult
                {
                    Data = hostInfo,
                    Status = ServiceStatus.Ok,
                    Type = ServiceType.ReverseDNS,
                };
            }
            catch (SocketException e)
            {
                return new ServiceResult
                {
                    Type = ServiceType.ReverseDNS,
                    Status = ServiceStatus.Error,
                    ErrorMessage =
                        $@"Socket Exception. This typically means the address wasn't able to be looked up. 
                        Detailed Error: {e}"
                };
            }
            catch (FormatException e)
            {
                return new ServiceResult
                {
                    Type = ServiceType.ReverseDNS,
                    Status = ServiceStatus.Error,
                    ErrorMessage = e.ToString(),
                };
            }
        }
    }
}