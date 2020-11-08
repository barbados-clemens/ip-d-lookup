using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IpDLookUp.Services.Models
{
    /// <summary>
    /// https://api.ssllabs.com/api/v3/analyze?host=calebukle.com
    /// Wait for the status of "READY" for valid results
    /// </summary>
    public struct SslLabsModel
    {
        [JsonPropertyName("host")]
        public string Host { get; set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }

        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }

        /// <summary>
        /// When "READY" then results will be valid
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// This is retuned as an epoch, might need to return as int instead
        /// </summary>
        [JsonPropertyName("startTime")]
        public long StartTime { get; set; }

        [JsonPropertyName("testTime")]
        public long TestTime { get; set; }

        [JsonPropertyName("engineVersion")]
        public string EngineVersion { get; set; }

        [JsonPropertyName("criteriaVersion")]
        public string CriteriaVersion { get; set; }

        [JsonPropertyName("endpoints")]
        public List<SslLabsEndpointModels> Endpoints { get; set; }
    }

    public struct SslLabsEndpointModels
    {
        [JsonPropertyName("ipAddress")]
        public string IpAddress { get; set; }

        [JsonPropertyName("serverName")]
        public string ServerName { get; set; }

        [JsonPropertyName("statusMessage")]
        public string StatusMessage { get; set; }

        [JsonPropertyName("grade")]
        public string Grade { get; set; }

        [JsonPropertyName("gradeTrustIgnored")]
        public string GradeTrustIgnored { get; set; }

        [JsonPropertyName("hasWarnings")]
        public bool HasWarnings { get; set; }

        [JsonPropertyName("isExceptional")]
        public bool IsExceptional { get; set; }

        [JsonPropertyName("progress")]
        public int Progress { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("delegation")]
        public int Delegation { get; set; }
    }
}
