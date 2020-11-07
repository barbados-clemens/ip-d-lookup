using System.Net;
using System.Net.NetworkInformation;

namespace IpDLookUp.Services.Models
{
    public struct PingModel
    {
        public IPAddress Address { get; set; }

        public long RoundTripTime { get; set; }

        public int BufferSize { get; set; }

        public PingOptions Options { get; set; }

        public IPStatus Status { get; set; }
    }
}