﻿using System;

namespace expense_api.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CostCentre { get; set; }
        public string ApproverId { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ExpenseItem[] ExpenseItems { get; set; }
    }
}
