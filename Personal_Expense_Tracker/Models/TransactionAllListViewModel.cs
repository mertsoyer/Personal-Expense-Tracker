using System;
using System.Collections.Generic;

namespace Personal_Expense_Tracker.Models
{
    public class TransactionAllListViewModel
    {
        public TransactionAllListViewModel()
        {
            Incomes = new List<Transaction>();
            Expenses = new List<Transaction>();
            Categories = new List<Category>();
        }
        public List<Transaction> Incomes { get; set; }
        public List<Transaction> Expenses { get; set; }
        public List<Category> Categories { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public int CategoryId { get; set; }
        public decimal IncomeSum { get; set; }
        public decimal ExpenseSum { get; set; }
        //transaction controlleri için ekleme
        public string Type { get; set; }
    }
}
