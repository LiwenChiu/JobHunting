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
using Microsoft.AspNetCore.Authorization;

namespace JobHunting.Areas.Admins.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminScheme", Roles = "admin")]
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
                PayDate = co.PayDate,
                Duration = co.Duration,
                Intro = co.Plan.Intro,
                Status = co.Status,
            }));
        }

        ////POST: Admins/CompanyOrders/BootFilterPage
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public async Task<IEnumerable<CompanyOrdersFilterViewModel>> BootFilterPage([FromBody][Bind("OrderId,CompanyId,PlanId,CompanyName,GUINumber,Title,Price,PayDate,Duration,Intro,Status")] CompanyOrdersViewModel covm)
        //{
        //    CultureInfo cultureTW = new CultureInfo("zh-TW");

        //    return _context.CompanyOrders.Include(co => co.Plan).Select(co => new
        //    {
        //        OrderId = co.OrderId,
        //        CompanyId = co.CompanyId,
        //        PlanId = co.PlanId,
        //        CompanyName = co.CompanyName,
        //        GUINumber = co.GUINumber,
        //        Title = co.Title,
        //        Price = co.Price,
        //        PayDate = co.PayDate.ToString(),
        //        Duration = co.Duration,
        //        Expiration = co.Status ? co.PayDate.AddDays(co.Duration).ToString() : null,
        //        Intro = co.Plan.Intro,
        //        Status = co.Status,
        //    }).Where(covmfilter => covmfilter.OrderId.ToString().Contains(covm.OrderId.ToString()) ||
        //                            covmfilter.CompanyId.ToString().Contains(covm.CompanyId.ToString()) ||
        //                            covmfilter.PlanId.ToString().Contains(covm.PlanId.ToString()) ||
        //                            covmfilter.CompanyName.Contains(covm.CompanyName) ||
        //                            covmfilter.GUINumber.Contains(covm.GUINumber) ||
        //                            covmfilter.Title.Contains(covm.Title) ||
        //                            covmfilter.Price.ToString().Contains(covm.Price.ToString()) ||
        //                            covmfilter.PayDate.Contains(covm.PayDate) ||
        //                            covmfilter.Duration.ToString().Contains(covm.Duration.ToString()) ||
        //                            covmfilter.Expiration.Contains(covm.Expiration) ||
        //                            covmfilter.Intro.Contains(covm.Intro) ||
        //                            (covmfilter.Status ? "已付款" : "尚未付款").Contains(covm.Status))
        //    .Select(co => new CompanyOrdersFilterViewModel
        //    {
        //        OrderId = co.OrderId,
        //        CompanyId = co.CompanyId,
        //        PlanId = co.PlanId,
        //        CompanyName = co.CompanyName,
        //        GUINumber = co.GUINumber,
        //        Title = co.Title,
        //        Price = co.Price,
        //        PayDate = co.PayDate,
        //        //PayDate = DateTime.Parse(co.PayDate).ToString(cultureTW),
        //        Duration = co.Duration,
        //        Expiration = co.Expiration == null ? "無" : co.Expiration,
        //        //Expiration = DateTime.Parse(co.Expiration).ToString(cultureTW),
        //        Intro = co.Intro,
        //        Status = co.Status,
        //    });
        //}

        //POST: Admins/CompanyOrders/BootFilterPaging
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<CompanyOrdersOutputViewModel> BootFilterPaging([FromBody][Bind("OrderId,CompanyId,PlanId,CompanyName,GUINumber,Title,Price,PayDate,Duration,Intro,Status,PageDraw,PageLength,PageStart")] CompanyOrdersViewModel covm)
        {
            CultureInfo cultureTW = new CultureInfo("zh-TW");

            var companyorders = _context.CompanyOrders.Include(co => co.Plan).AsNoTracking()
                .Select(co => new
                {
                    OrderId = co.OrderId,
                    CompanyId = co.CompanyId,
                    PlanId = co.PlanId,
                    CompanyName = co.CompanyName,
                    GUINumber = co.GUINumber,
                    Title = co.Title,
                    Price = co.Price,
                    PayDate = co.PayDate.ToString(),
                    Duration = co.Duration,
                    Expiration = co.Status ? co.PayDate.AddDays(co.Duration).ToString() : null,
                    Intro = co.Plan.Intro,
                    Status = co.Status
                }).Where(covmfilter => covmfilter.OrderId.ToString().Contains(covm.OrderId.ToString()) ||
                                        covmfilter.CompanyId.ToString().Contains(covm.CompanyId.ToString()) ||
                                        covmfilter.PlanId.ToString().Contains(covm.PlanId.ToString()) ||
                                        covmfilter.CompanyName.Contains(covm.CompanyName) ||
                                        covmfilter.GUINumber.Contains(covm.GUINumber) ||
                                        covmfilter.Title.Contains(covm.Title) ||
                                        covmfilter.Price.ToString().Contains(covm.Price.ToString()) ||
                                        covmfilter.PayDate.Contains(covm.PayDate) ||
                                        covmfilter.Duration.ToString().Contains(covm.Duration.ToString()) ||
                                        covmfilter.Expiration.Contains(covm.Expiration) ||
                                        covmfilter.Intro.Contains(covm.Intro) ||
                                        (covmfilter.Status ? "已付款" : "尚未付款").Contains(covm.Status))
                .Select(co => new CompanyOrdersFilterViewModel
                {
                    OrderId = co.OrderId,
                    CompanyId = co.CompanyId,
                    PlanId = co.PlanId,
                    CompanyName = co.CompanyName,
                    GUINumber = co.GUINumber,
                    Title = co.Title,
                    Price = co.Price,
                    PayDate = co.PayDate,
                    //PayDate = DateTime.Parse(co.PayDate).ToString(cultureTW),
                    Duration = co.Duration,
                    Expiration = co.Expiration == null ? "無" : co.Expiration,
                    //Expiration = DateTime.Parse(co.Expiration).ToString(cultureTW),
                    Intro = co.Intro,
                    Status = co.Status,
                }).OrderBy(co => co.Status).ThenByDescending(co => co.OrderId);

            var filterPaging = new CompanyOrdersOutputViewModel
            {
                totalDataCount = companyorders.Count(),
                companyOrdersFilter = companyorders.Skip((covm.PageStart - 1) * covm.PageLength).Take(covm.PageLength),
            };

            return filterPaging;
        }
    }
}
