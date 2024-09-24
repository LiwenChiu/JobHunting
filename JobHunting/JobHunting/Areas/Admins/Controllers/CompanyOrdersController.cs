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
using Microsoft.IdentityModel.Tokens;

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

        //POST: Admins/CompanyOrders/BootFilterPaging
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<CompanyOrdersOutputViewModel> BootFilterPaging([FromBody][Bind("OrderId,CompanyId,PlanId,CompanyName,GUINumber,Title,Price,PayDate,Duration,Intro,StatusType,PageDraw,PageLength,PageStart")] CompanyOrdersViewModel covm)
        {
            var companyorders = _context.CompanyOrders.Include(co => co.Plan).AsNoTracking()
                .OrderByDescending(co => co.PayDate)
                .Select(co => new CompanyOrdersFilterViewModel
                {
                    OrderId = co.OrderId,
                    CompanyId = co.CompanyId,
                    PlanId = co.PlanId,
                    CompanyName = co.CompanyName,
                    GUINumber = co.GUINumber,
                    Title = co.Title,
                    Price = co.Price,
                    PayDate = co.StatusType == "付款成功" ? co.PayDate : null,
                    Duration = co.Duration,
                    Intro = co.Plan.Intro,
                    Status = co.Status,
                    StatusType = co.StatusType,
                }).Where(covmfilter => covmfilter.CompanyId.ToString().Contains(covm.CompanyId.ToString()) ||
                                        covmfilter.PlanId.ToString().Contains(covm.PlanId.ToString()) ||
                                        covmfilter.CompanyName.Contains(covm.CompanyName) ||
                                        covmfilter.GUINumber.Contains(covm.GUINumber) ||
                                        covmfilter.Title.Contains(covm.Title) ||
                                        covmfilter.Price.ToString().Contains(covm.Price.ToString()) ||
                                        covmfilter.PayDate.ToString().Contains(covm.PayDate) ||
                                        covmfilter.Duration.ToString().Contains(covm.Duration.ToString()) ||
                                        covmfilter.Intro.Contains(covm.Intro) ||
                                        covmfilter.StatusType.Contains(covm.StatusType))
                .OrderBy(co => co.Status)
                .Select(co => new CompanyOrdersFilterOutputViewModel
                {
                    OrderId = co.OrderId,
                    CompanyId = co.CompanyId,
                    PlanId = co.PlanId,
                    CompanyName = co.CompanyName,
                    GUINumber = co.GUINumber,
                    Title = co.Title,
                    Price = co.Price,
                    PayDate = co.PayDate.HasValue ? co.PayDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "無",
                    Duration = co.Duration,
                    StatusType = co.StatusType.IsNullOrEmpty() ? "尚未付款" : co.StatusType,
                });

            var filterPaging = new CompanyOrdersOutputViewModel
            {
                totalDataCount = companyorders.Count(),
                companyOrdersFilter = companyorders.Skip((covm.PageStart - 1) * covm.PageLength).Take(covm.PageLength),
            };

            return filterPaging;
        }
    }
}
