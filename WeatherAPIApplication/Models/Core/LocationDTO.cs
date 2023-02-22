using System.Text.Json.Serialization;

namespace WeatherAPIApplication.Models.Core
{
    public class LocationDTO
    {
        [JsonPropertyName("name")]
        public string CityName { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
        [JsonPropertyName("lat")]
        public decimal Latitude { get; set; }
        [JsonPropertyName("lon")]
        public decimal Longitude { get; set; }
    }
}