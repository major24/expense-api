using System;

namespace expense_api.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string TransType { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public string Category { get; set; }
        public string CategoryDescription { get; set; }
        public DateTime TransDate { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
