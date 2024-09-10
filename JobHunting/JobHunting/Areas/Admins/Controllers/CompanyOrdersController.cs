using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Admins.ViewModels;
using System.Numerics;
using Microsoft.AspNetCore.Authentication.Cookies;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using System.Drawing.Configuration;
using Castle.Components.DictionaryAdapter.Xml;

namespace JobHunting.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class CompanyOrdersController : Controller
    {
        private readonly DuckAdminsContext _context;

        public CompanyOrdersController(DuckAdminsContext context)
        {
            _context = context;
        }

        // GET: Admins/CompanyOrders
        public IActionResult Index()
        {
            //static DateTime Expiration(DateTime date, int DurationTime)
            //{
            //    System.TimeSpan Duration = new System.TimeSpan(DurationTime, 0, 0, 0);
            //    System.DateTime ExpirationTime = date.Add(Duration);

            //    return ExpirationTime;
            //}

            return View();
        }

        // GET: Admins/CompanyOrders/IndexJson
        public async Task<JsonResult> IndexJson()
        {
            return Json(_context.CompanyOrders.Include(co => co.Plan).Select(co => new
            {
                OrderId = co.OrderId,
                CompanyId = co.CompanyId,
                PlanId = co.PlanId,
                CompanyName = co.CompanyName,
                GUINumber = co.GUINumber,
                Title = co.Title,
                Price = co.Price,
                OrderDate = co.OrderDate,
                Duration = co.Duration,
                Intro = co.Plan.Intro,
                Status = co.Status,
            }));
        }

        //POST: Admins/CompanyOrders/BootFilterPage
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<CompanyOrdersOutputViewModel>> BootFilterPage([FromBody][Bind("OrderId,CompanyId,PlanId,CompanyName,GUINumber,Title,Price,OrderDate,Duration,Intro,Status")] CompanyOrdersViewModel covm)
        {
            CultureInfo cultureTW = new CultureInfo("zh-TW");

            return _context.CompanyOrders.Include(co => co.Plan).Select(co => new
            {
                OrderId = co.OrderId,
                CompanyId = co.CompanyId,
                PlanId = co.PlanId,
                CompanyName = co.CompanyName,
                GUINumber = co.GUINumber,
                Title = co.Title,
                Price = co.Price,
                OrderDate = co.OrderDate.ToString(),
                Duration = co.Duration,
                Expiration = co.OrderDate.AddDays(co.Duration).ToString(),
                Intro = co.Plan.Intro,
                Status = co.Status,
            }).Where(covmfilter => covmfilter.OrderId.ToString().Contains(covm.OrderId.ToString()) ||
                                    covmfilter.CompanyId.ToString().Contains(covm.CompanyId.ToString()) ||
                                    covmfilter.PlanId.ToString().Contains(covm.PlanId.ToString()) ||
                                    covmfilter.CompanyName.Contains(covm.CompanyName) ||
                                    covmfilter.GUINumber.Contains(covm.GUINumber) ||
                                    covmfilter.Title.Contains(covm.Title) ||
                                    covmfilter.Price.ToString().Contains(covm.Price.ToString()) ||
                                    covmfilter.OrderDate.Contains(covm.OrderDate) ||
                                    covmfilter.Duration.ToString().Contains(covm.Duration.ToString()) ||
                                    covmfilter.Expiration.Contains(covm.Expiration) ||
                                    covmfilter.Intro.Contains(covm.Intro) ||
                                    (covmfilter.Status ? "已付款" : "尚未付款").Contains(covm.Status))
            .Select(co => new CompanyOrdersOutputViewModel
            {
                OrderId = co.OrderId,
                CompanyId = co.CompanyId,
                PlanId = co.PlanId,
                CompanyName = co.CompanyName,
                GUINumber = co.GUINumber,
                Title = co.Title,
                Price = co.Price,
                OrderDate = co.OrderDate,
                //OrderDate = DateTime.Parse(co.OrderDate).ToString(cultureTW),
                Duration = co.Duration,
                Expiration = co.Expiration,
                //Expiration = DateTime.Parse(co.Expiration).ToString(cultureTW),
                Intro = co.Intro,
                Status = co.Status,
            });
        }

        //POST: Admins/CompanyOrders/BootFilterPaging
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> BootFilterPaging([FromBody][Bind("OrderId,CompanyId,PlanId,CompanyName,GUINumber,Title,Price,OrderDate,Duration,Intro,Status,PageDraw,PageLength,PageStart")] CompanyOrdersViewModel covm)
        {
            CultureInfo cultureTW = new CultureInfo("zh-TW");

            return Json(new
            {
                draw = covm.PageDraw,
                total = _context.CompanyOrders.Count(),
                data =
                (_context.CompanyOrders.Include(co => co.Plan).Select(co => new
                {
                    OrderId = co.OrderId,
                    CompanyId = co.CompanyId,
                    PlanId = co.PlanId,
                    CompanyName = co.CompanyName,
                    GUINumber = co.GUINumber,
                    Title = co.Title,
                    Price = co.Price,
                    OrderDate = co.OrderDate.ToString(),
                    Duration = co.Duration,
                    Expiration = co.OrderDate.AddDays(co.Duration).ToString(),
                    Intro = co.Plan.Intro,
                    Status = co.Status,
                }).Where(covmfilter => covmfilter.OrderId.ToString().Contains(covm.OrderId.ToString()) ||
                                        covmfilter.CompanyId.ToString().Contains(covm.CompanyId.ToString()) ||
                                        covmfilter.PlanId.ToString().Contains(covm.PlanId.ToString()) ||
                                        covmfilter.CompanyName.Contains(covm.CompanyName) ||
                                        covmfilter.GUINumber.Contains(covm.GUINumber) ||
                                        covmfilter.Title.Contains(covm.Title) ||
                                        covmfilter.Price.ToString().Contains(covm.Price.ToString()) ||
                                        covmfilter.OrderDate.Contains(covm.OrderDate) ||
                                        covmfilter.Duration.ToString().Contains(covm.Duration.ToString()) ||
                                        covmfilter.Expiration.Contains(covm.Expiration) ||
                                        covmfilter.Intro.Contains(covm.Intro) ||
                                        (covmfilter.Status ? "已付款" : "尚未付款").Contains(covm.Status))
                .Select(co => new CompanyOrdersOutputViewModel
                {
                    OrderId = co.OrderId,
                    CompanyId = co.CompanyId,
                    PlanId = co.PlanId,
                    CompanyName = co.CompanyName,
                    GUINumber = co.GUINumber,
                    Title = co.Title,
                    Price = co.Price,
                    OrderDate = co.OrderDate,
                    //OrderDate = DateTime.Parse(co.OrderDate).ToString(cultureTW),
                    Duration = co.Duration,
                    Expiration = co.Expiration,
                    //Expiration = DateTime.Parse(co.Expiration).ToString(cultureTW),
                    Intro = co.Intro,
                    Status = co.Status,
                })).Skip(covm.PageStart).Take(covm.PageLength)});
        }
    }
}
