﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace expense_api.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ManagerId { get; set; }
        public string CostCentre { get; set; }
        public string Active { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
