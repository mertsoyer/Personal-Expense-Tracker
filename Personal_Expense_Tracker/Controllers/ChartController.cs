using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Personal_Expense_Tracker.Models;
using Personal_Expense_Tracker.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Newtonsoft.Json;

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
            var model = new TransactionListViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Index([FromForm] TransactionListViewModel transactionListViewModel, string btnsearch)
        {

            return Json(TransactionListWithFilter(transactionListViewModel, btnsearch));


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


        public IActionResult VisuliazeTransactionResult(TransactionListViewModel transactionListViewModel, string btnsearch)
        {
            if (transactionListViewModel == null) transactionListViewModel = new TransactionListViewModel();
            return Json(TransactionListWithFilter(transactionListViewModel, btnsearch));
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
                    transactionamount = item.Sum(l => l.Amount),
                });
            }

            ChartsViewModel model = new ChartsViewModel();
            model.gelirler = gelirler;
            model.giderler = giderler;

            return model;
        }

        public ChartsViewModel TransactionListWithFilter(TransactionListViewModel transactionListViewModel, string btnsearch = "")
        {
            List<Class> gelirler = new List<Class>();
            List<Class> giderler = new List<Class>();

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

            var giderler2 = query.ToList();

            var groupedgiderler = giderler2.GroupBy(x => x.Category.Name).ToList();

            foreach (var item in groupedgiderler)
            {
                giderler.Add(new Class()
                {
                    categoryname = item.Key,
                    transactionamount = item.Sum(l => -l.Amount),
                });
            }

            var query2 = context.Transactions.Include(x => x.Category).Where(l => l.UserId == id && l.Amount >= 0);


            if (transactionListViewModel.StartDate != default(DateTime))
            {
                query2 = query2.Where(x => x.Date >= transactionListViewModel.StartDate);
            }
            if (transactionListViewModel.EndDate != default(DateTime))
            {
                query2 = query2.Where(x => x.Date <= transactionListViewModel.EndDate);

            }

            var gelirler2 = query2.ToList();

            var groupedgelirler = gelirler2.GroupBy(x => x.Category.Name).ToList();

            foreach (var item in groupedgelirler)
            {
                gelirler.Add(new Class()
                {
                    categoryname = item.Key,
                    transactionamount = item.Sum(l => l.Amount),
                });
            }

            ChartsViewModel model = new ChartsViewModel();
            model.gelirler = gelirler;
            model.giderler = giderler;

            return model;
        }

        public IActionResult Index2()
        {

            var result = new List<List<dynamic>>();
            var id = _userManager.GetUserId(User);

            var nowDt = DateTime.Now;
            var culture = new CultureInfo("TR-tr");

            var yearAgoDt = nowDt.AddYears(-1).AddMonths(1);
            var searchYearDateUser = new DateTime(yearAgoDt.Year, yearAgoDt.Month, 1);
            var subList = new List<dynamic>();
            subList.Add("Aylar");
            subList.Add("Gelirler");
            subList.Add("Giderler");
            result.Add(subList);

            for (var i = 0; i <= 12; i++)
            {
                 subList = new List<dynamic>();

                subList.Add(searchYearDateUser.Date.ToString("Y", culture));

                var transactions = context.Transactions.Where(l => l.UserId == id && l.Date >= searchYearDateUser && l.Date < searchYearDateUser.AddMonths(1)).ToList();

                var gelirler = Convert.ToInt32(transactions.Where(l => l.Amount >= 0).Sum(l => l.Amount));
                var giderler = Convert.ToInt32(transactions.Where(l => l.Amount < 0).Sum(l => l.Amount));

                subList.Add(gelirler);
                subList.Add(giderler);
                result.Add(subList);
              
                searchYearDateUser = searchYearDateUser.AddMonths(1);
            }
            return View("Index2",JsonConvert.SerializeObject(result));
        }

        public virtual async Task<IActionResult> LoadOrderStatistics(string period)
        {

            var result = new List<List<string>>();
            var id = _userManager.GetUserId(User);

            var nowDt = DateTime.Now;
            var culture = new CultureInfo("TR-tr");

            var yearAgoDt = nowDt.AddYears(-1).AddMonths(1);
            var searchYearDateUser = new DateTime(yearAgoDt.Year, yearAgoDt.Month, 1);

            for (var i = 0; i <= 12; i++)
            {
                var subList = new List<string>();

                subList.Add(searchYearDateUser.Date.ToString("Y", culture));

                var transactions = context.Transactions.Where(l => l.UserId == id && l.Date >= searchYearDateUser && l.Date < searchYearDateUser.AddMonths(1)).ToList();

                var gelirler = transactions.Where(l => l.Amount >= 0).Sum(l=>l.Amount).ToString();
                var giderler = transactions.Where(l => l.Amount < 0).Sum(l => l.Amount).ToString();

                subList.Add(gelirler);
                subList.Add(giderler);
                result.Add(subList);
                //result.Add(new List<string>());
                //{
                //    date = searchYearDateUser.Date.ToString("Y", culture),
                //    value = (await _orderService.SearchOrdersAsync(
                //        createdFromUtc: _dateTimeHelper.ConvertToUtcTime(searchYearDateUser, timeZone),
                //        createdToUtc: _dateTimeHelper.ConvertToUtcTime(searchYearDateUser.AddMonths(1), timeZone),
                //        pageIndex: 0,
                //        pageSize: 1, getOnlyTotalCount: true)).TotalCount.ToString()
                //});

            searchYearDateUser = searchYearDateUser.AddMonths(1);
        }

            //switch (period)
            //{
            //    case "year":
            //        //year statistics
            //        var yearAgoDt = nowDt.AddYears(-1).AddMonths(1);
            //        var searchYearDateUser = new DateTime(yearAgoDt.Year, yearAgoDt.Month, 1);
            //        for (var i = 0; i <= 12; i++)
            //        {
            //            result.Add(new
            //            {
            //                date = searchYearDateUser.Date.ToString("Y", culture),
            //                value = (await _orderService.SearchOrdersAsync(
            //                    createdFromUtc: _dateTimeHelper.ConvertToUtcTime(searchYearDateUser, timeZone),
            //                    createdToUtc: _dateTimeHelper.ConvertToUtcTime(searchYearDateUser.AddMonths(1), timeZone),
            //                    pageIndex: 0,
            //                    pageSize: 1, getOnlyTotalCount: true)).TotalCount.ToString()
            //            });

            //            searchYearDateUser = searchYearDateUser.AddMonths(1);
            //        }

            //        break;
            //    case "month":
            //        //month statistics
            //        var monthAgoDt = nowDt.AddDays(-30);
            //        var searchMonthDateUser = new DateTime(monthAgoDt.Year, monthAgoDt.Month, monthAgoDt.Day);
            //        for (var i = 0; i <= 30; i++)
            //        {
            //            result.Add(new
            //            {
            //                date = searchMonthDateUser.Date.ToString("M", culture),
            //                value = (await _orderService.SearchOrdersAsync(
            //                    createdFromUtc: _dateTimeHelper.ConvertToUtcTime(searchMonthDateUser, timeZone),
            //                    createdToUtc: _dateTimeHelper.ConvertToUtcTime(searchMonthDateUser.AddDays(1), timeZone),
            //                    pageIndex: 0,
            //                    pageSize: 1, getOnlyTotalCount: true)).TotalCount.ToString()
            //            });

            //            searchMonthDateUser = searchMonthDateUser.AddDays(1);
            //        }

            //        break;
            //    case "week":
            //    default:
            //        //week statistics
            //        var weekAgoDt = nowDt.AddDays(-7);
            //        var searchWeekDateUser = new DateTime(weekAgoDt.Year, weekAgoDt.Month, weekAgoDt.Day);
            //        for (var i = 0; i <= 7; i++)
            //        {
            //            result.Add(new
            //            {
            //                date = searchWeekDateUser.Date.ToString("d dddd", culture),
            //                value = (await _orderService.SearchOrdersAsync(
            //                    createdFromUtc: _dateTimeHelper.ConvertToUtcTime(searchWeekDateUser, timeZone),
            //                    createdToUtc: _dateTimeHelper.ConvertToUtcTime(searchWeekDateUser.AddDays(1), timeZone),
            //                    pageIndex: 0,
            //                    pageSize: 1, getOnlyTotalCount: true)).TotalCount.ToString()
            //            });

            //            searchWeekDateUser = searchWeekDateUser.AddDays(1);
            //        }

            //        break;
            //}

            return Json(result);
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
