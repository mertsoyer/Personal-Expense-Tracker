using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal_Expense_Tracker.Data;
using Personal_Expense_Tracker.Models;
using System;
using System.Linq;

namespace Personal_Expense_Tracker.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        private UserManager<IdentityUser> _userManager;

        public TransactionController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var id = _userManager.GetUserId(User);
            var gelirler = context.Transactions.Include(x => x.Category).Where(l => l.UserId == id && l.Amount >= 0).ToList();

            foreach (var item in gelirler)
            {
                item.FormatedDate = item.Date.ToShortDateString();
            }

            var listModel = new TransactionAllListViewModel();

            listModel.Incomes = gelirler;
            listModel.IncomeSum = gelirler.Sum(x => x.Amount);

            var giderler = context.Transactions.Include(x => x.Category).Where(l => l.UserId == id && l.Amount < 0).ToList();

            foreach (var item in giderler)
            {
                item.FormatedDate = item.Date.ToShortDateString();
            }

            listModel.Expenses = giderler;
            listModel.ExpenseSum = giderler.Sum(x => x.Amount);

            return View(listModel);
        }

        [HttpPost]
        public IActionResult Index(TransactionAllListViewModel transactionAllListViewModel, string btnsearch)
        {

            var id = _userManager.GetUserId(User);

            if (btnsearch == "today")
            {
                transactionAllListViewModel.StartDate = DateTime.Today;
                transactionAllListViewModel.EndDate = DateTime.Today;
            }
            if (btnsearch == "thisWeek")
            {
                var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);


                transactionAllListViewModel.StartDate = monday;
                transactionAllListViewModel.EndDate = DateTime.Today;
            }
            if (btnsearch == "thisMounth")
            {
                var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                transactionAllListViewModel.StartDate = firstDayOfMonth;
                transactionAllListViewModel.EndDate = DateTime.Today;
            }
            if (btnsearch == "allTime")
            {
                transactionAllListViewModel.StartDate = default(DateTime);
                transactionAllListViewModel.EndDate = default(DateTime);
            }

            var query = context.Transactions.Include(x => x.Category).Where(l => l.UserId == id );

            if (transactionAllListViewModel.StartDate != default(DateTime))
            {
                query = query.Where(x => x.Date >= transactionAllListViewModel.StartDate);
            }
            if (transactionAllListViewModel.EndDate != default(DateTime))
            {
                query = query.Where(x => x.Date <= transactionAllListViewModel.EndDate);

            }
            var gelirler = query.Where(x=>x.Amount >= 0).ToList();
            foreach (var item in gelirler)
            {
                item.FormatedDate = item.Date.ToShortDateString();
            }

            transactionAllListViewModel.Incomes = gelirler;


            transactionAllListViewModel.IncomeSum = gelirler.Sum(x => x.Amount);

            var giderler = query.Where(x => x.Amount < 0).ToList();
            foreach (var item in giderler)
            {
                item.FormatedDate = item.Date.ToShortDateString();
            }

            transactionAllListViewModel.Expenses = giderler;


            transactionAllListViewModel.ExpenseSum = giderler.Sum(x => x.Amount);


            return View(transactionAllListViewModel);
        }
    }
}
