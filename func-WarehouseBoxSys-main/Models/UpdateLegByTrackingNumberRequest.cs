using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;



namespace func_WarehouseBoxSys.Models
{
    public class UpdateLegByTrackingNumberRequest
    {
        [JsonPropertyName("tracking_number")]
        public string TrackingNumber { get; set; } = null!;

        [JsonPropertyName("status_details")]
        public string StatusCode { get; set; } = null!;

        [JsonPropertyName("substatus/code")]
        public string SubStatusCode { get; set; } = null!;

        [JsonPropertyName("updated_by")]
        public string UpdatedBy { get; set; } = null!;

        [JsonPropertyName("transaction")]
        public string? TransactionId { get; set; }
        [JsonPropertyName("StatusDate")]
        public string? StatusDate { get; set; }

    }

}
