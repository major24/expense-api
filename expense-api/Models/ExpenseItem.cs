using System;

namespace expense_api.Models
{
    public class ExpenseItem
    {
        public int Id { get; set; }
        public string TransType { get; set; }
        public int ExpenseId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public string Category { get; set; }
        public DateTime TransDate { get; set; }
    }
}
