﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenSharing.API.Models
{
    [Table("Account", Schema = "identity")]
    public class Account
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }

        public bool IsEnabled { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsConsentAccepted { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? DisabledDate { get; set; }
        public DateTime? ConsentDate { get; set; }

        public Guid AccountTypeId { get; set; }
        public virtual AccountType AccountType { get; set; }

        public virtual IList<AccountLocation> AccountLocations { get; set; } = new List<AccountLocation>();
    }
}