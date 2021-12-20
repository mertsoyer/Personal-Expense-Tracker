using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personal_Expense_Tracker.Controllers
{
    public class ExpenseController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
