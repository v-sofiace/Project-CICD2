using System.Text.Json.Serialization;

namespace func_WarehouseBoxSys.Models
{
        public class ParentServiceLevel
        {
            [JsonPropertyName("name")]
            public required string Name { get; set; }

            [JsonPropertyName("terms")]
            public required string Terms { get; set; }

            [JsonPropertyName("token")]
            public required string Token { get; set; }

            [JsonPropertyName("extended_token")]
            public required string ExtendedToken { get; set; }
    }
    
}
