using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("Account")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountID { get; set; }
        [StringLength(50)]
        [Required]
        public string UserName { get; set; }
        [StringLength(50)]
        [Required]
        public string Password { get; set; }
        [StringLength(250)]
        [Required]
        public string Email { get; set; }
        [Required]
        public int Warning { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual Role Roles { get; set; }
        public bool? status { get; set; }
        public virtual AccountDetail AccountDetails { get; set; }
        public virtual ICollection<Notication> Notications { get; set; } = new List<Notication>();
        public virtual ICollection<RegistAuctioneer> RegistAuctioneers { get; set; } = new List<RegistAuctioneer>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<ListAuctioneer> CreatedAuctioneers { get; set; } = new List<ListAuctioneer>();

        public virtual ICollection<ListAuctioneer> ManagedAuctioneers { get; set; } = new List<ListAuctioneer>();
    }
}
