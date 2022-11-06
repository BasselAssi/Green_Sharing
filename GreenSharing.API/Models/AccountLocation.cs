﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenSharing.API.Models
{
    [Table("AccountLocation", Schema = "location")]
    public class AccountLocation
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }

        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}

