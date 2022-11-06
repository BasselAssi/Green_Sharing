﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenSharingAPI.Models
{
    public class BankFoodProductConsumable
    {
        public Guid Id { get; set; }
        public string Notes { get; set; }
        public long Quantity { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        //FK
        public Guid BankFoodId { get; set; }
        public Guid ProductId { get; set; }

        public virtual Product Produit{ get; set; }
        public virtual BankFood BankFood { get; set; }
    }
}
