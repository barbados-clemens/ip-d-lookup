using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;

namespace IpDLookUp.Services.Models
{
    public struct PingModel
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("roundTripTime")]
        public long RoundTripTime { get; set; }

        [JsonPropertyName("bufferSize")]
        public int BufferSize { get; set; }

        [JsonPropertyName("options")]
        public PingOptions Options { get; set; }

        [JsonPropertyName("status")]
        public IPStatus Status { get; set; }
    }
}