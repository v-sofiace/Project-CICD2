using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace func_WarehouseBoxSys.Models
{
    public class TransactionEventData
    {
        public string? CommercialInvoiceUrl { get; set; }

        public CreatedBy? CreatedBy { get; set; }

        public string? Eta { get; set; }

        public string? LabelFileType { get; set; }

        public string? LabelUrl { get; set; }

        public List<Message>? Messages { get; set; }

        public string? Metadata { get; set; }

        public DateTime ObjectCreated { get; set; }

        public string? ObjectId { get; set; }

        public string? ObjectOwner { get; set; }

        public string? ObjectState { get; set; }

        public DateTime ObjectUpdated { get; set; }

        public string? Parcel { get; set; }

        public string? QrCodeUrl { get; set; }

        public Rate? Rate { get; set; }

        public required string Status { get; set; }

        public bool Test { get; set; }

        public string? TrackingNumber { get; set; }

        public required string TrackingStatus { get; set; }

        public string? TrackingUrlProvider { get; set; }
    }

}
