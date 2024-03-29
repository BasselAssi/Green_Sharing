﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenSharing.API.Repositories.DataAccessLayer.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public Guid ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }
    }
}
