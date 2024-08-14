﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Notication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoticationID { get; set; }
        [ForeignKey("Account")]
        public int AccountID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual Account Accounts { get; set; }
    }
}
