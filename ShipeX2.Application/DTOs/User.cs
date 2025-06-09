using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ShipeX2.Application.DTOs
{
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public long UserId { get; set; }
    }

    public class ModelUser
    {
        public int Id { get; set; }
        [Display(Name = "User Id")]
        [Required(ErrorMessage = "User Id is required")]
        public string? UserId { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public bool Status { get; set; }
        [Display(Name = "Role")]
        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }
        public string? Role { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        public int ShipXUid { get; set; }
        [Display(Name = "Label Printer Name")]
        [Required(ErrorMessage = "Label Printer Name is required")]
        public int LabelPntId { get; set; }

        [Display(Name = "Invoice Printer Name")]
        [Required(ErrorMessage = "Invoice Printer Name is required")]
        public int InvoicePntId { get; set; }
        public List<ModelShipXUser>? ShipXUserlist { get; set; }
        public List<userrole> userrolelist { get; set; }
        public List<ModelPrinter> LabelPrinterList { get; set; }
        public List<ModelPrinter> InvoicePrinterList { get; set; }
    }

    public class ModelShipXUser
    {
        public long ShipXUid { get; set; }
        public int LabelPntId { get; set; }
        public int InvoicePntId { get; set; }
        public int Id { get; set; }
        public int Createdby { get; set; }
        public DateTime Createddate { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? Modifieddate { get; set; }
    }
    public class userrole
    {
        public int RoleId { get; set; }
        public string Role { get; set; }
    }

    public class ModelPrinter
    {
        public int PntId { get; set; }

        [Display(Name = "Printer Name")]
        [Required(ErrorMessage = "Printer Alias Name is required")]
        public string? PntAliasName { get; set; }

        [Display(Name = "Printer Type")]
        [Required(ErrorMessage = "Printer Type is required")]
        public string? PntType { get; set; }

        [Display(Name = "Printer Mode")]
        [Required(ErrorMessage = "Printer Mode is required")]
        public string? PntMode { get; set; }

        [Display(Name = "Printer IP")]
        [Required(ErrorMessage = "Printer IP is required")]
        public string? PntIP { get; set; }


        [Display(Name = "Printer Identifier")]
        [Required(ErrorMessage = "Identifier is required")]
        public string? PntIdentifier { get; set; }


        [Display(Name = "Printer Cups")]
        [Required(ErrorMessage = "Printer Cups is required")]
        public string? CupsPrinterName { get; set; }

        public bool? Status { get; set; }

        public int Createdby { get; set; }
        public DateTime Createddate { get; set; }
        public int Modifiedby { get; set; }
        public DateTime Modifieddate { get; set; }

        public List<string> PrinterList { get; set; }

    }
}
