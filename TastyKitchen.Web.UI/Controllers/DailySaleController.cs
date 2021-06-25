using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neubel.Wow.Win.Authentication.Core.Interfaces.TastyKitchen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TastyKitchen.Web.UI.Models;

namespace TastyKitchen.Web.UI.Controllers
{
    public class DailySaleController : Controller
    {
        private readonly ILogger<DailySaleController> _logger;
        private readonly IDailySaleService _dailySaleService;
        public DailySaleController(ILogger<DailySaleController> logger, IDailySaleService dailySaleService)
        {
            _logger = logger;
            _dailySaleService = dailySaleService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var sales = _dailySaleService.GetPages(1);

            List<DailySale> dailySales = new List<DailySale>();

            foreach (var item in sales)
            {
                dailySales.Add(new DailySale { Id = item.Id, Amount = item.Amount, Date = item.Date, BillNumber = item.BillNumber, SaleType = item.Type });
            }
            ViewBag.nextPage = 2;
            ViewBag.PreviousPage = 0;
            return View(dailySales.AsEnumerable());
        }

        [HttpPost]
        public IActionResult Index(int pageIndex)
        {
            var expenses = _dailySaleService.GetPages(pageIndex);

            List<DailySale> dailySales = new List<DailySale>();

            foreach (var item in expenses)
            {
                dailySales.Add(new DailySale { Id = item.Id, Amount = item.Amount, Date = item.Date, BillNumber = item.BillNumber, SaleType = item.Type });
            }
            ViewBag.nextPage = pageIndex + 1;
            ViewBag.PreviousPage = pageIndex == 1 ? 1 : pageIndex - 1;
            return View(dailySales.AsEnumerable());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DailySale { Date = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] DailySale dailySale)
        {
            if (ModelState.IsValid)
            {
                var dailySaleBusinessModel = new Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen.DailySale
                {
                    Date = dailySale.Date,
                    Amount = dailySale.Amount,
                    Id = dailySale.Id,
                    BillNumber = dailySale.BillNumber,
                    Type = dailySale.SaleType
                };

                _dailySaleService.Add(dailySaleBusinessModel);

                return RedirectToAction("Index");
            }
            return View(dailySale);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = _dailySaleService.Get((int)id);

            if (sale == null)
            {
                return NotFound();
            }

            var expenseUIModel = new DailySale
            {
                Date = sale.Date,
                Amount = sale.Amount,
                Id = sale.Id,
                BillNumber = sale.BillNumber,
                SaleType = sale.Type
            };

            return View(expenseUIModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] DailySale dailySale)
        {
            if (id != dailySale.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var expenseBusinessModel = new Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen.DailySale
                {
                    Date = dailySale.Date,
                    Amount = dailySale.Amount,
                    Id = dailySale.Id,
                    BillNumber = dailySale.BillNumber,
                    Type = dailySale.SaleType
                };

                _dailySaleService.Update(id, expenseBusinessModel);

                return RedirectToAction("Index");
            }
            return View(dailySale);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = _dailySaleService.Get((int)id);

            if (sale == null)
            {
                return NotFound();
            }

            var saleUIModel = new DailySale
            {
                Date = sale.Date,
                Amount = sale.Amount,
                Id = sale.Id,
                BillNumber = sale.BillNumber,
                SaleType = sale.Type
            };

            return View(saleUIModel);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var sale = _dailySaleService.Get((int)id);


            if (sale == null)
            {
                return NotFound();
            }


            var saleUIModel = new DailySale
            {
                Date = sale.Date,
                Amount = sale.Amount,
                Id = sale.Id,
                BillNumber = sale.BillNumber,
                SaleType = sale.Type
            };
            return View(saleUIModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _dailySaleService.Delete((int)id);
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
    }
}
