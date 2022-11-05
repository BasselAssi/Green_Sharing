﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenSharingAPI.Models
{
    public class BankFoodReview
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        //FK
        public Guid BankFoodID { get; set; }
        public Guid AccountID { get; set; }
        public virtual Account Account { get; set; }
        public virtual BankFood BankFood { get; set; }
    }
}