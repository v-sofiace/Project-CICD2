using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Text.Json.Serialization;
namespace func_WarehouseBoxSys.Models
{
    public class CarrierTrackingEvent
    {
        [JsonPropertyName("event")]
        public required string Event { get; set; }

        [JsonPropertyName("test")]
        public required bool Test { get; set; }

        [JsonPropertyName("data")]
        public required TrackEventData Data { get; set; }
    }
}
