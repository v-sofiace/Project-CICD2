using System.Text.Json.Serialization;

namespace func_WarehouseBoxSys.Models
{
    public class TrackingHistory
    {
        [JsonPropertyName("location")]
        public required Location Location { get; set; }

        [JsonPropertyName("object_created")]
        public required DateTime ObjectCreated { get; set; }

        [JsonPropertyName("object_id")]
        public required string ObjectId { get; set; }

        [JsonPropertyName("object_updated")]
        public required DateTime ObjectUpdated { get; set; }

        [JsonPropertyName("status")]
        public required string Status { get; set; }

        [JsonPropertyName("substatus")]
        public required Substatus Substatus { get; set; }

        [JsonPropertyName("status_date")]
        public required DateTime StatusDate { get; set; }

        [JsonPropertyName("status_details")]
        public required string StatusDetails { get; set; }
    }
}
