using System.Text.Json.Serialization;

namespace func_WarehouseBoxSys.Models
{
    public class Location
    {
        [JsonPropertyName("city")]
        public required string City { get; set; }

        [JsonPropertyName("country")]
        public required string Country { get; set; }

        [JsonPropertyName("state")]
        public required string State { get; set; }

        [JsonPropertyName("zip")]
        public required string Zip { get; set; }
    }
}
