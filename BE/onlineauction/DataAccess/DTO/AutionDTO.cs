﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class AutionDTO
    {
        public int ListAuctionID { get; set; }
        public string owner { get; set; }
        public string Image { get; set; }
        public string NameAuction { get; set; }
        public decimal moneyDeposit { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public string StatusAuction { get; set; }
        public string StartDay { get; set; }
        public string StartTime { get; set; }
        public string EndDay { get; set; }
        public string EndTime { get; set; }
        public int? NumberofAuctionRounds { get; set; }
        public string TimePerLap { get; set; }
        public decimal? PriceStep { get; set; }
        public string PaymentMethod { get; set; }
        public string FileAuctioneer { get; set; }
        public string SignatureImg { get; set; }
        public object TImage { get; set; }
        public string countdowntime { get; set; }
        public List<Image> images { get; set; }
        public string createDate { get; set; }
    }
}
