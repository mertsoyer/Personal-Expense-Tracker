using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Personal_Expense_Tracker.Models;
using Personal_Expense_Tracker.Data;

namespace Personal_Expense_Tracker.Controllers
{
    
    
    public class ChartController : Controller

    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VisuliazeTransactionResult()
        {

            return Json(TransactionList());
        }

        public IActionResult Index2()
        {
            return View();
        }
        public List<Class>TransactionList()

        {

            List<Class> class2 =new List<Class>();
            using (var context2 = new ApplicationDbContext())
            {
                class2 = context2.Transactions.Select(x => new Class
                {
                    TransactionAmount = x.Amount,
                    CategoryName = x.Category.ToString()
                }).ToList();

                return class2;
            }
        }
    }
}
