using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Text.Json.Serialization;
namespace func_WarehouseBoxSys.Models
{
    public class CarrierTrackingEvent
    {
        [JsonPropertyName("event")]
        public string Event { get; set; }

        [JsonPropertyName("test")]
        public bool Test { get; set; }

        [JsonPropertyName("data")]
        public TrackEventData Data { get; set; }
    }
}
