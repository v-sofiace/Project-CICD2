using func_WarehouseBoxSys.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace func_WarehouseBoxSys.Services
{
    public static class JsonUtilityHelper
    {
        // write a method to convert json data to an object of type CarrierTrackingEvent model
        public static CarrierTrackingEvent ConvertJsonToCarrierTrackingEvent(string json)
        {
            return JsonConvert.DeserializeObject<CarrierTrackingEvent>(json);
        }

        // write a method to convert an object of type CarrierTrackingEvent to json data
        public static string ConvertCarrierTrackingEventToJson(CarrierTrackingEvent carrierTrackingEvent)
        {
            return JsonConvert.SerializeObject(carrierTrackingEvent);
        }
    }
}
