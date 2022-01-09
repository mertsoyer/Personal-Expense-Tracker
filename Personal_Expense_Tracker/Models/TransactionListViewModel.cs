using System;
using System.Collections.Generic;

namespace Personal_Expense_Tracker.Models
{
    public class TransactionListViewModel
    {
        public TransactionListViewModel()
        {
            Transactions = new List<Transaction>();
            Categories = new List<Category>();
        }
        public List<Transaction> Transactions { get; set; }
        public List<Category> Categories { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public int CategoryId { get; set; }
        public decimal TransactionSum { get; set; }
    }
}
