using System.Text.Json.Serialization;

namespace func_WarehouseBoxSys.Models
{
    public class TrackingStatus
    {
        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("object_created")]
        public DateTime ObjectCreated { get; set; }

        [JsonPropertyName("object_id")]
        public string ObjectId { get; set; }

        [JsonPropertyName("object_updated")]
        public DateTime ObjectUpdated { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("substatus")]
        public Substatus Substatus { get; set; }

        [JsonPropertyName("status_date")]
        public DateTime StatusDate { get; set; }

        [JsonPropertyName("status_details")]
        public string StatusDetails { get; set; }
    }
}
