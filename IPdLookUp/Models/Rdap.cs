using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IPdLookUp.Models
{
    public struct Rdap
    {
        [JsonPropertyName("status")]
        public List<string> Status { get; set; }

        [JsonPropertyName("events")]
        public List<RdapEvent> Events { get; set; }

        [JsonPropertyName("secureDNS")]
        public RdapSecureDNS SecureDns { get; set; }

        [JsonPropertyName("nameservers")]
        public List<RdapNameServers> Nameservers { get; set; }

        [JsonPropertyName("rdapConformance")]
        public List<string> RdapConformance { get; set; }
    }

    public struct RdapEvent
    {
        [JsonPropertyName("eventAction")]
        public string EventAction { get; set; }

        [JsonPropertyName("eventDate")]
        public DateTime EventDate { get; set; }
    }

    public struct RdapSecureDNS
    {
        [JsonPropertyName("delegationSigned")]
        public bool DelegationSigned { get; set; }

        [JsonPropertyName("dsData")]
        public List<object> DSData { get; set; }
    }

    public struct RdapDsData
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

    public struct RdapNameServers
    {
        [JsonPropertyName("objectClassName")]
        public string ObjectClassName { get; set; }

        [JsonPropertyName("ldhName")]
        public string LdhName { get; set; }
    }

    public struct RdapEntity
    {
        [JsonPropertyName("objectClassName")]
        public string ObjectClassName { get; set; }

        [JsonPropertyName("handle")]
        public string Handle { get; set; }

        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }

        [JsonPropertyName("rdapPublicIds")]
        public List<RdapPublicId> RdapPublicIds { get; set; }

        // TODO vcardArray

        [JsonPropertyName("entities")]
        public List<RdapEntity> Entities { get; set; }
    }

    public struct RdapPublicId
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }
    }
}