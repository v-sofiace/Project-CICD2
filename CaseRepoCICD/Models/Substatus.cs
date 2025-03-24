using System.Text.Json.Serialization;

namespace func_WarehouseBoxSys.Models
{
    public class Substatus
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("action_required")]
        public bool ActionRequired { get; set; }
    }
}
