using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IpDLookUp.Services.Models
{
    /// <summary>
    /// DTO for RDAP service API call
    /// </summary>
    public struct RdapModel
    {
        [JsonPropertyName("status")]
        public List<string> Status { get; set; }

        [JsonPropertyName("events")]
        public List<RdapEventModel> Events { get; set; }

        [JsonPropertyName("secureDNS")]
        public RdapSecureDNSModel SecureDnsModel { get; set; }

        [JsonPropertyName("nameservers")]
        public List<RdapNameServersModel> Nameservers { get; set; }

        [JsonPropertyName("rdapConformance")]
        public List<string> RdapConformance { get; set; }
    }

    public struct RdapEventModel
    {
        [JsonPropertyName("eventAction")]
        public string EventAction { get; set; }

        [JsonPropertyName("eventDate")]
        public DateTime EventDate { get; set; }
    }

    public struct RdapSecureDNSModel
    {
        [JsonPropertyName("delegationSigned")]
        public bool DelegationSigned { get; set; }

        [JsonPropertyName("dsData")]
        public List<object> DSData { get; set; }
    }

    public struct RdapDsDataModel
    {
        [JsonPropertyName("keyTag")]
        public int KeyTag { get; set; }

        [JsonPropertyName("algorithm")]
        public int Algorithm { get; set; }

        [JsonPropertyName("digestType")]
        public int DigestType { get; set; }

        [JsonPropertyName("digest")]
        public string Digest { get; set; }
    }

    public struct RdapNameServersModel
    {
        [JsonPropertyName("objectClassName")]
        public string ObjectClassName { get; set; }

        [JsonPropertyName("ldhName")]
        public string LdhName { get; set; }
    }

    public struct RdapEntityModel
    {
        [JsonPropertyName("objectClassName")]
        public string ObjectClassName { get; set; }

        [JsonPropertyName("handle")]
        public string Handle { get; set; }

        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }

        [JsonPropertyName("rdapPublicIds")]
        public List<RdapPublicIdModel> RdapPublicIds { get; set; }

        // TODO vcardArray

        [JsonPropertyName("entities")]
        public List<RdapEntityModel> Entities { get; set; }
    }

    public struct RdapPublicIdModel
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }
    }
}