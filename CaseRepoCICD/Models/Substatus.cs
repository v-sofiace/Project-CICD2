using System.Text.Json.Serialization;

namespace func_WarehouseBoxSys.Models
{
    public class Substatus
    {
        [JsonPropertyName("code")]
        public required string Code { get; set; }

        [JsonPropertyName("text")]
        public required string Text { get; set; }

        [JsonPropertyName("action_required")]
        public required bool ActionRequired { get; set; }
    }
}
