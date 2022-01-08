using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        private UserManager<IdentityUser> _userManager;


        public IncomeController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            

            var id = _userManager.GetUserId(User);
            var gelirler = context.Transactions.Include(x => x.Category).Where(l => l.UserId == id && l.Amount > 0).ToList();
           
            foreach (var item in gelirler)
            {
                item.FormatedDate = item.Date.ToShortDateString();
            }
            //var gelirler= context.Transactions.Where(x => x.Amount>=0).ToList();
            
            
            //var gelirler = context.Transactions.ToList();

            //var query = entity.M_Employee
            //      .SelectMany(e => entity.M_Position
            //                          .Where(p => e.PostionId >= p.PositionId));

            return View(gelirler);
        }

        [HttpGet]
        public IActionResult AddIncome()
        {

            List<SelectListItem> kategoriler = (from x in context.Categories.Where(l => l.Type == "income").ToList()
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
        public IActionResult AddIncome(Transaction transaction)
        {
            var id = _userManager.GetUserId(User);


            var yeniIslem = context.Categories.Where(x => x.CategoryId == transaction.Category.CategoryId)
                .FirstOrDefault();
            transaction.Category = yeniIslem;
            transaction.Amount = +transaction.Amount;
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
    }
}
