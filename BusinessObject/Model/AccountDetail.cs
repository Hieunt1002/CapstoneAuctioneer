using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class AccountDetail
    {
        [Key, ForeignKey("Account")]
        public int AccountID { get; set; }
        public virtual Account Accounts { set; get; }
        public string Avatar { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string FrontCCCD { get; set; }
        public string BacksideCCCD { get; set; }
        public string City { get; set; }
        public string ward { get; set; }
        public string district { get; set; }
        public string Address { get; set; }
    }
}
