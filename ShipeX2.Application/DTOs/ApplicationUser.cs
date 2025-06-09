using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipeX2.Application.DTOs
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public string AlternatePhoneNo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string UserCode { get; set; }
        public string LastLoginIP { get; set; }
        public string LastUserAgent { get; set; }
        public int LoginCount { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
    }


    [Table("users", Schema = "public")]
    public class UserModel
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public string? center_name { get; set; }

        public string? center_code { get; set; }

        public string? address { get; set; }

        public string? pin_code { get; set; }

        public string? alternate_phone_no { get; set; }

        public string? user_code { get; set; }

        public string? user_name { get; set; }

        public string? normalized_user_name { get; set; }

        public string? email { get; set; }

        public string? normalized_email { get; set; }

        public bool email_confirmed { get; set; }

        public string? password_hash { get; set; }

        public string? security_stamp { get; set; }

        public string? ConcurrencyStamp { get; set; }

        public string? phone_number { get; set; }

        public bool phone_number_confirmed { get; set; }

        public bool two_factor_enabled { get; set; }

        public DateTimeOffset? lockout_end { get; set; }

        public bool lockout_enabled { get; set; }

        public int access_failed_count { get; set; }

        public DateTime created_date { get; set; }
        public string? created_by { get; set; }

        public DateTime? modify_date { get; set; }

        public string? modify_by { get; set; }

        public string? last_user_agent { get; set; }

        public string? last_login_ip { get; set; }

        public int login_count { get; set; }

        public bool is_deleted { get; set; }

        public bool is_active { get; set; }
    }

}
