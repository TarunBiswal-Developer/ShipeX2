using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipeX2.Persistence.TableModels
{
    public class Tables
    {
        [Table("SPX_tbluserrole", Schema = "public")]
        public class UserRole
        {
            [Key]
            [Column("RoleId")]
            public long RoleId { get; set; }

            [Column("Role")]
            public string? Role { get; set; }

            [Column("Status")]
            public bool? Status { get; set; }
        }

        [Table("SPX_tblloginCredential", Schema = "public")]
        public class LoginCredential
        {
            [Key]
            [Column("Id")]
            public long Id { get; set; }

            [Column("UserId")]
            public required string UserId { get; set; }

            [Column("Password")]
            public string? Password { get; set; }

            [Column("Status")]
            public bool? Status { get; set; }

            [Column("RoleId")]
            public long? RoleId { get; set; }

            [Column("Name")]
            public string? Name { get; set; }

            [Column("PrinterIP")]
            public string? PrinterIP { get; set; }

            [Column("CreatedBy")]
            public long? CreatedBy { get; set; }

            [Column("CreatedDate")]
            public DateTime? CreatedDate { get; set; }

            [Column("UpdatedBy")]
            public long? UpdatedBy { get; set; }

            [Column("UpdatedDate")]
            public DateTime? UpdatedDate { get; set; }
        }

        [Table("SPX_tblShipCarrier", Schema = "public")]
        public class ShipCarrier
        {
            [Key]
            public long CarrierId { get; set; }
            public string? CarrierName { get; set; }
            public string? DefaultAccountNo { get; set; }
            public string? ApiKey1 { get; set; }
            public string? ApiKey2 { get; set; }
            public string? ApiKey3 { get; set; }
            public long? CreatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
            public long? ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool? Status { get; set; }
            public bool? Mode { get; set; }
        }

        [Table("SPX_tblPrinter", Schema = "public")]
        public class Printer
        {
            [Key]
            public long PntId { get; set; }
            public string PntAliasName { get; set; }
            public string PntType { get; set; }
            public string PntMode { get; set; }
            public string PntIP { get; set; }
            public string PntIdentifier { get; set; }
            public bool Status { get; set; }
            public long Createdby { get; set; }
            public DateTime Createddate { get; set; }
            public long? Modifiedby { get; set; }
            public DateTime? Modifieddate { get; set; }
            public string CupsPrinterName { get; set; }
        }

        [Table("SPX_tblShipXUser", Schema = "public")]
        public class ShipXUser
        {
            [Key]
            public long ShipXUid { get; set; }
            public long? LabelPntId { get; set; }
            public long? InvoicePntId { get; set; }
            public long? Id { get; set; }
            public long? Createdby { get; set; }
            public DateTime? Createddate { get; set; }
            public long? Modifiedby { get; set; }
            public DateTime? Modifieddate { get; set; }
        }

        [Table("SPX_tblimporter", Schema = "public")]
        public class Importer
        {
            [Key]
            public long ImporterId { get; set; }
            public long ClientId { get; set; }
            public string BrokerName { get; set; }
            public string Country { get; set; }
            public string ImporterName { get; set; }
            public string ImporterCompany { get; set; }
            public string ImporterAddress1 { get; set; }
            public string ImporterAddress2 { get; set; }
            public string ImporterState { get; set; }
            public string ImporterCity { get; set; }
            public string ImporterPostCode { get; set; }
            public string ImporterPhone { get; set; }
            public bool Status { get; set; }
            public long CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public long ModifiedBy { get; set; }
            public DateTime ModifiedDate { get; set; }
            public string BrokerCompany { get; set; }
            public string BrokerAddress1 { get; set; }
            public string BrokerAddress2 { get; set; }
            public string BrokerPhone { get; set; }
            public string BrokerCity { get; set; }
            public string BrokerState { get; set; }
            public string BrokerPostalCode { get; set; }
            public string ImporterCountry { get; set; }
            public string BrokerCountry { get; set; }
        }

        [Table("SPX_tblClient", Schema = "public")]
        public class Client
        {
            [Key]
            public long ClientId { get; set; }
            public string Name { get; set; }
            public string ClientCode { get; set; }
            public string CompanyName { get; set; }
            public string Phone1 { get; set; }
            public string Phone2 { get; set; }
            public string Email { get; set; }
            public string Addresline1 { get; set; }
            public string Addresline2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public string PostalCode { get; set; }
            public string ShipName { get; set; }
            public string ShipCompany { get; set; }
            public string ShipAddress1 { get; set; }
            public string ShipAddress2 { get; set; }
            public string ShipCity { get; set; }
            public string ShipState { get; set; }
            public string ShipCounty { get; set; }
            public string ShipPostcode { get; set; }
            public string ShipPhone1 { get; set; }
            public string ShipPhone2 { get; set; }
            public string ShipEmailId { get; set; }
            public bool Status { get; set; }
            public long CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public long? ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public string PrinterIP { get; set; }
            public string STDCode { get; set; }
            public string ShipmentType { get; set; }
            public string MeterNo { get; set; }
            public string FedExMGR { get; set; }
            public string ComInvCopy { get; set; }
            public bool IsDTC { get; set; }
        }
    }
}
