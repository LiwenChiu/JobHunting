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

        //POST: Admins/CompanyOrders/BootFilterPaging
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<CompanyOrdersOutputViewModel> BootFilterPaging([FromBody][Bind("OrderId,CompanyId,PlanId,CompanyName,GUINumber,Title,Price,PayDate,Duration,Intro,Status,PageDraw,PageLength,PageStart")] CompanyOrdersViewModel covm)
        {
            var companyorders = _context.CompanyOrders.Include(co => co.Plan).AsNoTracking()
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
                    Duration = co.Duration,
                    Expiration = co.Status ? co.PayDate.AddDays(co.Duration) : null,
                    Intro = co.Plan.Intro,
                    Status = co.Status
                }).Where(covmfilter => covmfilter.CompanyId.ToString().Contains(covm.CompanyId.ToString()) ||
                                        covmfilter.PlanId.ToString().Contains(covm.PlanId.ToString()) ||
                                        covmfilter.CompanyName.Contains(covm.CompanyName) ||
                                        covmfilter.GUINumber.Contains(covm.GUINumber) ||
                                        covmfilter.Title.Contains(covm.Title) ||
                                        covmfilter.Price.ToString().Contains(covm.Price.ToString()) ||
                                        covmfilter.PayDate.ToString().Contains(covm.PayDate) ||
                                        covmfilter.Duration.ToString().Contains(covm.Duration.ToString()) ||
                                        covmfilter.Expiration.ToString().Contains(covm.Expiration) ||
                                        covmfilter.Intro.Contains(covm.Intro) ||
                                        (covmfilter.Status ? "已付款" : "尚未付款").Contains(covm.Status))
                .OrderBy(co => co.PayDate)
                .Select(co => new CompanyOrdersFilterOutputViewModel
                {
                    OrderId = co.OrderId,
                    CompanyId = co.CompanyId,
                    PlanId = co.PlanId,
                    CompanyName = co.CompanyName,
                    GUINumber = co.GUINumber,
                    Title = co.Title,
                    Price = co.Price,
                    PayDate = co.PayDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    Duration = co.Duration,
                    Expiration = co.Expiration.HasValue ? co.Expiration.Value.ToString("yyyy/MM/dd HH:mm:ss") : "無",
                    Intro = co.Intro,
                    Status = co.Status,
                }).OrderBy(co => co.Status);

            var filterPaging = new CompanyOrdersOutputViewModel
            {
                totalDataCount = companyorders.Count(),
                companyOrdersFilter = companyorders.Skip((covm.PageStart - 1) * covm.PageLength).Take(covm.PageLength),
            };

            return filterPaging;
        }
    }
}
