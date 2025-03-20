using System.Text.Json.Serialization;

namespace func_WarehouseBoxSys.Models
{
    public class ServiceLevel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("terms")]
        public string Terms { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("extended_token")]
        public string ExtendedToken { get; set; }

        [JsonPropertyName("parent_servicelevel")]
        public ParentServiceLevel ParentServicelevel { get; set; }
    }
}
