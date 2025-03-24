using System.Net;
using System.Text.Json.Serialization;

namespace func_WarehouseBoxSys.Models
{
    public class TrackEventData
    {
        [JsonPropertyName("address_from")]
        public required Address AddressFrom { get; set; }

        [JsonPropertyName("address_to")]
        public required Address AddressTo { get; set; }

        [JsonPropertyName("carrier")]
        public required string Carrier { get; set; }

        [JsonPropertyName("eta")]
        public required DateTime Eta { get; set; }

        [JsonPropertyName("messages")]
        public required List<string> Messages { get; set; }

        [JsonPropertyName("metadata")]
        public required string Metadata { get; set; }

        [JsonPropertyName("original_eta")]
        public required DateTime OriginalEta { get; set; }

        [JsonPropertyName("servicelevel")]
        public required ServiceLevel Servicelevel { get; set; }

        [JsonPropertyName("tracking_history")]
        public required  List<TrackingHistory> TrackingHistory { get; set; }

        [JsonPropertyName("tracking_number")]
        public required  string TrackingNumber { get; set; }

        [JsonPropertyName("tracking_status")]
        public required TrackingStatus TrackingStatus { get; set; }

        [JsonPropertyName("transaction")]
        public required string Transaction { get; set; }
    }
}
