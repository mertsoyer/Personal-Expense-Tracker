using Microsoft.AspNetCore.Mvc;
using Personal_Expense_Tracker.Data;
using Personal_Expense_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personal_Expense_Tracker.Controllers
{
    public class IncomeController : Controller
    {

        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index()
        {
           var gelirler= context.Transactions.Where(x => x.Amount>=0).ToList();
            
            
            //var gelirler = context.Transactions.ToList();

            //var query = entity.M_Employee
            //      .SelectMany(e => entity.M_Position
            //                          .Where(p => e.PostionId >= p.PositionId));

            return View(gelirler);
        }


        public IActionResult AddIncome()
        {


            return View();
        }


        [HttpPost]
        public IActionResult AddIncome(Transaction transaction)
        {



            return View(transaction);
        }
    }
}
