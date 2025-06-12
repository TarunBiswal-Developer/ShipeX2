using System.ComponentModel.DataAnnotations;

namespace ShipeX2.Application.DTOs
{
    public class ModelShipCarrier
    {

        public int CarrierId { get; set; }

        [Display(Name = "Carrier Name")]
        [Required(ErrorMessage = "Carrier Name is required")]
        public required string CarrierName { get; set; }

        [Display(Name = "Account No.")]
        [Required(ErrorMessage = "Account No is required")]
        public required string DefaultAccountNo { get; set; }

        [Display(Name = "API Key(1)")]
        [Required(ErrorMessage = "ApiKey(1) is required")]
        public string? ApiKey1 { get; set; }

        [Display(Name = "API Key(2)")]
        public string? ApiKey2 { get; set; }

        [Display(Name = "API Key(3)")]
        public string? ApiKey3 { get; set; }
        public bool? Status { get; set; }
        public bool? Mode { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public class ModelShipCarrierService
    {

        public int ServiceId { get; set; }
        [Display(Name = "Select Carrier")]
        [Required(ErrorMessage = "Carrier  is required")]
        public int CarrierId { get; set; }
        [Display(Name = "Service Name")]
        [Required(ErrorMessage = "Service Name is required")]
        public string ServiceName { get; set; }
        [Display(Name = "Ship Via Code")]
        [Required(ErrorMessage = "Service Code is required")]
        public string? ServiceCode { get; set; }

        [Display(Name = "Carrier Name")]
        public string? CarrierName { get; set; }

        [Display(Name = "FSM Service Code")]
        public string? FSMServiceCode { get; set; }
        public string? PackingName { get; set; }
        public bool? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<ModelShipCarrier>? ShipCarrierlist { get; set; }
        public List<ModelShipCarrierPacking>? Packinglist { get; set; }
        public List<ModelShipCarrierService>? ShipServicelist { get; set; }
    }

    public class ShipmentServiceModel
    {
        public List<string> ShipmentIds { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public int CarrierId { get; set; }
        public int? ServiceId { get; set; }
        public string? FSMServiceCode { get; set; }

    }

    public class ModelShipCarrierPacking
    {


        public int PackingId { get; set; }

        [Display(Name = "Select Carrier")]
        [Required(ErrorMessage = "Carrier  is required")]
        public int CarrierId { get; set; }

        [Display(Name = "Packing")]
        [Required(ErrorMessage = "Packing Name is required")]
        public string PackingName { get; set; }

        [Display(Name = "FSM Packing Code")]
        public string FSMPackingCode { get; set; }

        [Display(Name = "Height")]
        public decimal Height { get; set; }

        [Display(Name = "Width")]
        public decimal Width { get; set; }

        [Display(Name = "Length")]
        public decimal Length { get; set; }

        [Display(Name = "Max Weight")]
        [Required(ErrorMessage = "Enter Max Weight")]
        public decimal MaxWeight { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        public string CarrierName { get; set; }

        public bool Status { get; set; }

        public string chkStatus { get; set; }
        public int CreatedBy { get; set; }
        public int ServiceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<ModelShipCarrier> ShipCarrierlist { get; set; }



    }

}
