using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using System.Globalization;
using System.Numerics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace JobHunting.Areas.Companies.Controllers
{
    [Authorize(Roles = "company")]
    [Area("Companies")]
    public class CompanyOrdersController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public CompanyOrdersController(DuckCompaniesContext context)
        {
            _context = context;
        }

        // GET: Companies/CompanyOrders
        public IActionResult Index()
        {
            return View();
        }

        // GET: Companies/CompanyOrders/IndexJson
        [HttpGet]
        public JsonResult IndexJson(int id)
        {
            var Order = _context.CompanyOrders
                .Select(p => new
                {
                    OrderID = p.OrderId,
                    CompanyName = p.CompanyName,
                    GUINumber = p.GUINumber,
                    Title = p.Title,
                    Price = p.Price,
                    OrderDate = p.OrderDate,
                    Status = p.Status
                });

            return Json(Order);
        }

        //POST: Companies/CompanyOrders/GetCompanyOrders
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<CompanyOrdersFilterOutputViewModel>> GetCompanyOrders([FromBody][Bind("Title,Price,OrderDate,PayDate,Duration")] CompanyOrdersFilterViewModel cofvm)
        {
            CultureInfo cultureTW = new CultureInfo("zh-TW");
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return new List<CompanyOrdersFilterOutputViewModel>(); // 處理未授權的情況
            }
            return _context.CompanyOrders
                .Where(co => co.CompanyId.ToString() == CompanyId)
                .Where(co => co.Title.Contains(cofvm.Title) ||
                             co.Price.ToString().Contains(cofvm.Price.ToString()) ||
                             co.OrderDate.ToString().Contains(cofvm.OrderDate) ||
                             co.PayDate.ToString().Contains(cofvm.PayDate) ||
                             co.Duration.ToString().Contains(cofvm.Duration.ToString()))
                .Select(co => new CompanyOrdersFilterOutputViewModel
                {
                    OrderId = co.OrderId,
                    Title = co.Title,
                    Price = co.Price,
                    OrderDate = co.OrderDate,
                    PayDate = co.Status ? co.PayDate : null,
                    Duration = co.Duration,
                    Status = co.Status,
                })
                .OrderBy(co => co.Status).ThenByDescending(co => co.OrderId);
        }

        //POST: Companies/CompanyOrders/GetDeadline
        [HttpPost]
        public async Task<DateTime?> GetDeadline()
        {
            // 從 claims 中取得 CompanyId
            var companyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(companyId))
            {
                return null; // 處理未授權訪問的情況
            }

            var companyDeadline = _context.Companies.AsNoTracking().Where(c => c.CompanyId.ToString() == companyId).Select(c => c.Deadline).SingleOrDefault();
            return companyDeadline;
        }

        //POST: Companies/CompanyOrders/CancelOrder
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> CancelOrder([FromBody][Bind("CompanyId,OrderId")] CancelOrderViewModel covm)
        {
            string[] returnStatus = new string[2];
            var companyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(companyId))
            {
                returnStatus = ["未授權訪問", "失敗"];
                return returnStatus;
            }

            int parsedCompanyId;
            if (!int.TryParse(companyId, out parsedCompanyId))
            {
                returnStatus = ["公司 ID 無效", "失敗"];
                return returnStatus;
            }
            if (!ModelState.IsValid)
            {
                returnStatus = ["格式不正確", "失敗"];
                return returnStatus;
            }

            var order = await _context.CompanyOrders.FindAsync(covm.OrderId);
            if (order == null || order.CompanyId != parsedCompanyId)
            {
                returnStatus = ["訂單不存在", "失敗"];
                return returnStatus;
            }
            
            _context.CompanyOrders.Remove(order);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                returnStatus = ["取消訂單失敗!", "失敗"];
                return returnStatus;
            }

            returnStatus = ["取消訂單成功", "成功"];
            return returnStatus;

        }
    }
}
