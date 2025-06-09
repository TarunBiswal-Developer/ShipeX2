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
            public string? UserId { get; set; }

            [Column("Password")]
            public string? Password { get; set; }

            [Column("Status")]
            public bool? Status { get; set; }

            [Column("RoleId")]
            public long RoleId { get; set; }

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
    }
}
