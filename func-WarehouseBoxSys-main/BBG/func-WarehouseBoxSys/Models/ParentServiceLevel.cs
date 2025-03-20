using System.Text.Json.Serialization;

namespace func_WarehouseBoxSys.Models
{
        public class ParentServiceLevel
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("terms")]
            public string Terms { get; set; }

            [JsonPropertyName("token")]
            public string Token { get; set; }

            [JsonPropertyName("extended_token")]
            public string ExtendedToken { get; set; }
    }
    
}
