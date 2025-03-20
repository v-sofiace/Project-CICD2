using System.Net;
using System.Text.Json.Serialization;

namespace func_WarehouseBoxSys.Models
{
    public class TrackEventData
    {
        [JsonPropertyName("address_from")]
        public Address AddressFrom { get; set; }

        [JsonPropertyName("address_to")]
        public Address AddressTo { get; set; }

        [JsonPropertyName("carrier")]
        public string Carrier { get; set; }

        [JsonPropertyName("eta")]
        public DateTime Eta { get; set; }

        [JsonPropertyName("messages")]
        public List<string> Messages { get; set; }

        [JsonPropertyName("metadata")]
        public string Metadata { get; set; }

        [JsonPropertyName("original_eta")]
        public DateTime OriginalEta { get; set; }

        [JsonPropertyName("servicelevel")]
        public ServiceLevel Servicelevel { get; set; }

        [JsonPropertyName("tracking_history")]
        public List<TrackingHistory> TrackingHistory { get; set; }

        [JsonPropertyName("tracking_number")]
        public string TrackingNumber { get; set; }

        [JsonPropertyName("tracking_status")]
        public TrackingStatus TrackingStatus { get; set; }

        [JsonPropertyName("transaction")]
        public string Transaction { get; set; }
    }
}
