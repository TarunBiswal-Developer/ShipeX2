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
}
