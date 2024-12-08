﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class AcceptAutioneerDTO
    {
        public int AutioneerID { get; set; }
        public bool Status { get; set; }
        public string? TimeRoom { get; set; }
        public IFormFile? evidenceFile { get; set; }
    }
}
