using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neubel.Wow.Win.Authentication.Core.Interfaces.TastyKitchen;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TastyKitchen.Web.UI.Models;

namespace TastyKitchen.Web.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDailyExpenseService _dailyExpenseService;

        public HomeController(ILogger<HomeController> logger, IDailyExpenseService dailyExpenseService)
        {
            _logger = logger;
            _dailyExpenseService = dailyExpenseService;
        }

        [HttpGet]
        public IActionResult Index()
        {

            var expenses = _dailyExpenseService.GetPages(1);

            List<DailyExpense> dailyExpenses = new List<DailyExpense>();

            foreach (var item in expenses)
            {
                dailyExpenses.Add(new DailyExpense { Id = item.Id, Amount = item.Amount, Date = item.Date, Name = item.Name, Unit = item.Unit, Quantity = item.Quantity });
            }
            ViewBag.nextPage = 2;
            ViewBag.PreviousPage = 0;
            return View(dailyExpenses.AsEnumerable());
        }

        [HttpPost]
        public IActionResult Index(int pageIndex)
        {
            
            var expenses = _dailyExpenseService.GetPages(pageIndex);

            List<DailyExpense> dailyExpenses = new List<DailyExpense>();

            foreach (var item in expenses)
            {
                dailyExpenses.Add(new DailyExpense { Id = item.Id, Amount = item.Amount, Date = item.Date, Name = item.Name, Unit = item.Unit, Quantity = item.Quantity });
            }
            ViewBag.nextPage = pageIndex + 1;
            ViewBag.PreviousPage = pageIndex == 1 ? 1 : pageIndex - 1;
            return View(dailyExpenses.AsEnumerable());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DailyExpense { Date = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] DailyExpense dailyExpense)
        {
            if (ModelState.IsValid)
            {
                var dailyExpenseBusinessModel = new Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen.DailyExpense
                {
                    Date = dailyExpense.Date,
                    Unit = dailyExpense.Unit,
                    Amount = dailyExpense.Amount,
                    Id = dailyExpense.Id,
                    Name = dailyExpense.Name,
                    Quantity = dailyExpense.Quantity
                };

                _dailyExpenseService.Add(dailyExpenseBusinessModel);

                return RedirectToAction("Index");
            }
            return View(dailyExpense);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var expense = _dailyExpenseService.Get((int)id);


            if (expense == null)
            {
                return NotFound();
            }


            var expenseUIModel = new DailyExpense
            {
                Date = expense.Date,
                Unit = expense.Unit,
                Amount = expense.Amount,
                Id = expense.Id,
                Name = expense.Name,
                Quantity = expense.Quantity
            };

            return View(expenseUIModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] DailyExpense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var expenseBusinessModel = new Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen.DailyExpense
                {
                    Date = expense.Date,
                    Unit = expense.Unit,
                    Amount = expense.Amount,
                    Id = expense.Id,
                    Name = expense.Name,
                    Quantity = expense.Quantity
                };

                _dailyExpenseService.Update(id, expenseBusinessModel);

                return RedirectToAction("Index");
            }
            return View(expense);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            var expense = _dailyExpenseService.Get((int)id);

            if (expense == null)
            {
                return NotFound();
            }

            var expenseUIModel = new DailyExpense
            {
                Date = expense.Date,
                Unit = expense.Unit,
                Amount = expense.Amount,
                Id = expense.Id,
                Name = expense.Name,
                Quantity = expense.Quantity
            };

            return View(expenseUIModel);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var expense = _dailyExpenseService.Get((int)id);


            if (expense == null)
            {
                return NotFound();
            }


            var expenseUIModel = new DailyExpense
            {
                Date = expense.Date,
                Unit = expense.Unit,
                Amount = expense.Amount,
                Id = expense.Id,
                Name = expense.Name,
                Quantity = expense.Quantity
            };
            return View(expenseUIModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _dailyExpenseService.Delete((int)id);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin")]
        public IActionResult ExportToExcel()
        {
            List<DailyExpenseExcelDTO> resultsDTO = new List<DailyExpenseExcelDTO>();
            var results = _dailyExpenseService.Get();
            foreach (var item in results)
            {
                resultsDTO.Add(new DailyExpenseExcelDTO
                { 
                    Id = item.Id, 
                    Amount = item.Amount,
                    Date = item.Date.ToString("MM/dd/yyyy hh:mm tt"), 
                    Name = item.Name, 
                    Quantity = item.Quantity, 
                    Unit = item.Unit 
                });
            }

            var stream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.LoadFromCollection(resultsDTO, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"TastyKitchenExpense-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            return File(stream, "application/vdn.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
