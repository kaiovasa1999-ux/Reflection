﻿namespace Comparator.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public int ClinetId { get; set; }

        public int CreditSum { get; set; }

        public string Client { get; set; }

        public DateTime CDate { get; set; }

        public DateTime LDate { get; set; }

        
    }
}
