using System.Text.Json.Serialization;

namespace IPdLookUp.Models
{
    public struct GeoIp
    {
        [JsonPropertyName("ip")]
        public string IP { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }

        [JsonPropertyName("region_code")]
        public string? RegionCode { get; set; }

        [JsonPropertyName("region_name")]
        public string? RegionName { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("zip_code")]
        public string? ZipCode { get; set; }

        [JsonPropertyName("time_zone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("latitude")]
        public decimal Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public decimal Longitude { get; set; }

        [JsonPropertyName("metro_code")]
        public int MetroCode { get; set; }
    }
}