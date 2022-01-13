using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Personal_Expense_Tracker.Models;
using Personal_Expense_Tracker.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Personal_Expense_Tracker.Controllers
{
    public class ExpenseController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        private UserManager<IdentityUser> _userManager;

        public ExpenseController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var id = _userManager.GetUserId(User);
            var islemler = context.Transactions.Include(x => x.Category).Where(l => l.UserId == id && l.Amount < 0 ).ToList();

            foreach (var item in islemler)
            {
                item.FormatedDate = item.Date.ToShortDateString();
            }

            var listModel = new TransactionListViewModel();
            listModel.Transactions = islemler;
            listModel.TransactionSum = islemler.Sum(x => x.Amount);

            return View(listModel);
        }

        [HttpPost]
        public IActionResult Index([FromForm] TransactionListViewModel transactionListViewModel,string btnsearch)
        {
            var id = _userManager.GetUserId(User);

            if (btnsearch=="today")
            {
                transactionListViewModel.StartDate = DateTime.Today;
                transactionListViewModel.EndDate = DateTime.Today;
            }
            if (btnsearch=="thisWeek")
            {
                var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

               
                transactionListViewModel.StartDate = monday;
                transactionListViewModel.EndDate = DateTime.Today;
            }
            if (btnsearch== "thisMounth")
            {
                var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                transactionListViewModel.StartDate = firstDayOfMonth;
                transactionListViewModel.EndDate = DateTime.Today;
            }
            if (btnsearch=="allTime")
            {
                transactionListViewModel.StartDate=default(DateTime);
                transactionListViewModel.EndDate = default(DateTime);
            }
            
            var query = context.Transactions.Include(x => x.Category).Where(l => l.UserId == id && l.Amount < 0);
           
            if (transactionListViewModel.StartDate!=default(DateTime))
            {
                query = query.Where(x=> x.Date >= transactionListViewModel.StartDate);
            }
            if (transactionListViewModel.EndDate!=default(DateTime))
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


        [HttpGet]
        public IActionResult AddExpense()
        {
            
            List<SelectListItem> kategoriler = (from x in context.Categories.Where(l=> l.Type=="expense").ToList()
                                                select new SelectListItem
                                                {

                                                    Text = x.Name,
                                                    Value = x.CategoryId.ToString()
                                                }

            ).ToList();


            ViewBag.kategoriListele = kategoriler;



            return View();
        }


        [HttpPost]
        public IActionResult AddExpense(Transaction transaction)
        {
            var id = _userManager.GetUserId(User);


            var yeniIslem = context.Categories.Where(x => x.CategoryId == transaction.Category.CategoryId)
                .FirstOrDefault();
            transaction.Category = yeniIslem;
            transaction.Amount = -transaction.Amount;
            transaction.UserId = id;
            context.Transactions.Add(transaction);
            context.SaveChanges();


            (from Category in context.Categories select Category).ToList();
            List<SelectListItem> kategoriler = (from x in context.Categories.ToList()
                                                select new SelectListItem
                                                {

                                                    Text = x.Name,
                                                    Value = x.CategoryId.ToString()
                                                }

            ).ToList();


            ViewBag.kategoriListele = kategoriler;

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult DeleteExpense(int id)
        {
            var harcamaSil = context.Transactions.Find(id);

            context.Transactions.Remove(harcamaSil);
            context.SaveChanges();

            //var islemler = context.Transactions.Include(x => x.Category).ToList();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult UpdateExpense(int id)
        {
            List<SelectListItem> kategoriler = (from x in context.Categories.Where(l => l.Type == "expense").ToList()
                                                select new SelectListItem
                                                {

                                                    Text = x.Name,
                                                    Value = x.CategoryId.ToString()
                                                }

                       ).ToList();


            ViewBag.kategoriListele = kategoriler;
            var harcamaGuncelle = context.Transactions.Find(id);

            return View(harcamaGuncelle);
        }

        [HttpPost]

        public IActionResult UpdateExpense (Transaction updatedTransaction)
        {
            var newCategory = context.Categories.Where(x => x.CategoryId == updatedTransaction.Category.CategoryId)
                 .FirstOrDefault();

            var transaction = context.Transactions.Where(l => l.Id == updatedTransaction.Id).FirstOrDefault();
            transaction.Category = newCategory;
            transaction.Amount = +updatedTransaction.Amount;
            transaction.Date = updatedTransaction.Date;
            transaction.UserId = updatedTransaction.UserId;
            transaction.Name = updatedTransaction.Name;
            context.Transactions.Update(transaction);
            context.SaveChanges();


            (from Category in context.Categories select Category).ToList();
            List<SelectListItem> kategoriler = (from x in context.Categories.ToList()
                                                select new SelectListItem
                                                {

                                                    Text = x.Name,
                                                    Value = x.CategoryId.ToString()
                                                }

            ).ToList();


            ViewBag.kategoriListele = kategoriler;

            return RedirectToAction("Index");
        }
    }
}
