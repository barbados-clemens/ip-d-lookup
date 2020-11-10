using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using IpDLookUp.Services.Types;

namespace IpDLookUp.Services
{
    /// <summary>
    /// Service for processing Reverse DNS Requests
    /// This is only valid for IP addresses
    /// </summary>
    public class ReverseDns : Service<IPHostEntry>
    {
        public override async Task<IServiceResult<IPHostEntry>> DoLookUp(string address, AddressType type)
        {
            try
            {
                if (type != AddressType.Ip)
                {
                    return new ServiceResult<IPHostEntry>
                    {
                        Status = ServiceStatus.Bad,
                        Type = ServiceType.ReverseDNS,
                        ErrorMessage = "Only IP Addresses are valid for an Reverse DNS look up",
                        WorkerId = Environment.MachineName,
                    };
                }

                var hostIpAddress = IPAddress.Parse(address);
                var hostInfo = await Dns.GetHostEntryAsync(hostIpAddress);

                return new ServiceResult<IPHostEntry>
                {
                    Data = hostInfo,
                    Status = ServiceStatus.Ok,
                    Type = ServiceType.ReverseDNS,
                };
            }
            catch (SocketException e)
            {
                return new ServiceResult<IPHostEntry>
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
                return new ServiceResult<IPHostEntry>
                {
                    Type = ServiceType.ReverseDNS,
                    Status = ServiceStatus.Bad,
                    ErrorMessage = e.ToString(),
                };
            }
        }
    }
}