using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Personal_Expense_Tracker.Data;
using Personal_Expense_Tracker.Models;
using System.Collections.Generic;
using System.Linq;

namespace Personal_Expense_Tracker.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index()
        {
            var kategori=context.Categories.ToList();   

            return View(kategori);
        }

        public IActionResult DeleteCategory(int id)
        {
            var categorySil = context.Categories.Find(id);
            context.Categories.Remove(categorySil);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddCategory()
        {

            (from Category in context.Categories select Category).ToList();

            List<SelectListItem> kategoriler = (from x in context.Categories.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.Name,
                                                     Value = x.Type.ToString()
                                                 }
              ).ToList();

            ViewBag.departmanListele = kategoriler;

            return View();  
        }

        [HttpPost]
        public IActionResult AddCategory(Category category,string option)
        {
            if (option == "income")
            {
                context.Categories.Add(category);
                context.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateCategory (int id)
        {

            var kategoriGuncelle=context.Categories.Find(id);

            return View(kategoriGuncelle);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category category,string option)
        {
        
            var kategoriGuncelleme=context.Categories.Find(category.CategoryId);
            kategoriGuncelleme.Name = category.Name;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
