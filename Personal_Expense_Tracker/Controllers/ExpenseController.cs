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

        public IActionResult Index()
        {
            //var id = _userManager.GetUserId(User);
            //var islemler = context.Transactions.Where(l=>l.    Include(x => x.Category).ToList();

            //var islemler =context.Transactions.ToList();

            return View(/*islemler*/);
        }


        [HttpGet]
        public IActionResult AddExpense()
        {
            (from Category in context.Categories select Category).ToList();
            List<SelectListItem> kategoriler = (from x in context.Categories.ToList()
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

            var yeniIslem = context.Categories.Where(x => x.CategoryId == transaction.Category.CategoryId)
                .FirstOrDefault();
            transaction.Category = yeniIslem;
            transaction.Amount = -transaction.Amount;
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

            var islemler = context.Transactions.Include(x => x.Category).ToList();

            return View("Index",islemler);
        }


        [HttpGet]
        public IActionResult UpdateExpense(int id)
        {

            var harcamaGuncelle = context.Transactions.Find(id);


            return View(harcamaGuncelle);
        }

        [HttpPost]

        public IActionResult UpdateExpense (Transaction transaction)
        {
           var updateExpense= context.Transactions.Find(transaction.Id);
            updateExpense.Name= transaction.Name;
            context.SaveChanges();
            
          


            return View();
        }
    }
}
