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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm] TransactionListViewModel transactionListViewModel, string btnsearch)
        {

            var id = _userManager.GetUserId(User);

            if (btnsearch == "today")
            {
                transactionListViewModel.StartDate = DateTime.Today;
                transactionListViewModel.EndDate = DateTime.Today;
            }
            if (btnsearch == "thisWeek")
            {
                var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);


                transactionListViewModel.StartDate = monday;
                transactionListViewModel.EndDate = DateTime.Today;
            }
            if (btnsearch == "thisMounth")
            {
                var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                transactionListViewModel.StartDate = firstDayOfMonth;
                transactionListViewModel.EndDate = DateTime.Today;
            }
            if (btnsearch == "allTime")
            {
                transactionListViewModel.StartDate = default(DateTime);
                transactionListViewModel.EndDate = default(DateTime);
            }

            var query = context.Transactions.Include(x => x.Category).Where(l => l.UserId == id && l.Amount < 0);

            if (transactionListViewModel.StartDate != default(DateTime))
            {
                query = query.Where(x => x.Date >= transactionListViewModel.StartDate);
            }
            if (transactionListViewModel.EndDate != default(DateTime))
            {
                query = query.Where(x => x.Date <= transactionListViewModel.EndDate);

            }
            var islemler = query.ToList();
            foreach (var item in islemler)
            {
                item.FormatedDate = item.Date.ToShortDateString();
            }

            transactionListViewModel.Transactions = islemler;


            transactionListViewModel.TransactionSum = islemler.Sum(x => x.Amount);

            return View(transactionListViewModel);
           
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
