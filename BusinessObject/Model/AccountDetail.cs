using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class AccountDetail
    {
        [Key, ForeignKey("Account")]
        public string AccountID { get; set; }

        public virtual Account Account { get; set; }  

        [StringLength(250)]
        public string? Avatar { get; set; }

        [StringLength(100)]
        public string? FullName { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(50)]
        public string? FrontCCCD { get; set; }

        [StringLength(50)]
        public string? BacksideCCCD { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Ward { get; set; }  

        [StringLength(100)]
        public string? District { get; set; } 

        [StringLength(250)]
        public string? Address { get; set; }
    }
}