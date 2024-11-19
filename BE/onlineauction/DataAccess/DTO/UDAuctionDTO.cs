using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class UDAuctionDTO
    {
        public int auctionID { get; set; }
        public string? imageAuction { get; set; }
        public string? imageEvidence { get; set; }
        public string nameAuctionItem { get; set; }
        public string description { get; set; }
        public decimal startingPrice { get; set; }
        public int category { get; set; }
    }
}
