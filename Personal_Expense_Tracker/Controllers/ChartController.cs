using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Personal_Expense_Tracker.Models;
using Personal_Expense_Tracker.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Personal_Expense_Tracker.Controllers
{


    public class ChartController : Controller

    {
        ApplicationDbContext context = new ApplicationDbContext();
        private UserManager<IdentityUser> _userManager;

        public ChartController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult VisuliazeTransactionResult()
        {

            return Json(TransactionList());
        }

        public ChartsViewModel TransactionList()
        {
            List<Class> gelirler = new List<Class>();
            List<Class> giderler = new List<Class>();
            var id = _userManager.GetUserId(User);


            var giderler2 = context.Transactions.Include(x => x.Category).Where(l => l.UserId == id && l.Amount < 0).ToList();

            var groupedgiderler = giderler2.GroupBy(x => x.Category.Name).ToList();

            foreach (var item in groupedgiderler)
            {
                giderler.Add(new Class()
                {
                    categoryname = item.Key,
                    transactionamount = item.Sum(l => -l.Amount),
                });
            }

            var gelirler2 = context.Transactions.Include(x => x.Category).Where(l => l.UserId == id && l.Amount >= 0).ToList();

            var groupedgelirler = gelirler2.GroupBy(x => x.Category.Name).ToList();

            foreach (var item in groupedgelirler)
            {
                gelirler.Add(new Class()
                {
                    categoryname = item.Key,
                    transactionamount = item.Sum(l =>l.Amount),
                });
            }

            ChartsViewModel model = new ChartsViewModel();
            model.gelirler = gelirler;
            model.giderler= giderler;

            return model;
        }
        public IActionResult Index2()
        {
            return View();
        }
        //public List<Class>TransactionList()

        //{

        //    List<Class> class2 =new List<Class>();
        //    using (var context2 = new ApplicationDbContext())
        //    {
        //        class2 = context2.Transactions.Select(x => new Class
        //        {
        //            TransactionAmount = x.Amount,
        //            CategoryName = x.Category.ToString()
        //        }).ToList();

        //        return class2;
        //    }
        //}

        public IActionResult Index3()
        {
            return View();
        }

        public IActionResult VisualizeTransactionResult2()
        {
            return Json(TransactionList2());
        }
        public List<Class2> TransactionList2()
        {
            List<Class2> cs2 = new List<Class2>();
            using (var context = new ApplicationDbContext())
            {
                cs2 = context.Transactions.Select(t => new Class2
                {
                    transactionAmount = t.Amount,
                    transactionName = t.Name
                }).ToList();
            }
            return cs2;
        }

        
    }
}
