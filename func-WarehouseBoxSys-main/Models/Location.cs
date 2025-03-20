using System.Text.Json.Serialization;

namespace func_WarehouseBoxSys.Models
{
    public class Location
    {
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("zip")]
        public string Zip { get; set; }
    }
}
