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
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

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

        //POST: Companies/CompanyOrders/GetCompanyOrders
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<CompanyOrdersFilterOutputViewModel>> GetCompanyOrders([FromBody][Bind("Filter")] CompanyOrdersFilterViewModel cofvm)
        {
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return new List<CompanyOrdersFilterOutputViewModel>(); // 處理未授權的情況
            }
            int companyId = int.Parse(CompanyId);

            var query = _context.CompanyOrders.AsNoTracking()
            .Where(co => co.CompanyId == companyId)
            .Select(co => new CompanyOrdersFilterInputViewModel
            {
                OrderId = co.OrderId,
                OrderNumber = co.OrderNumber,
                CompanyId = co.CompanyId,
                Title = co.Title,
                Price = co.Price,
                OrderDate = co.OrderDate,
                PayDate = co.PayDate,
                Duration = co.Duration,
                Status = co.Status,
                StatusType = co.StatusType,
            });

            // Apply numeric filters dynamically
            if (int.TryParse(cofvm.Filter, out int filterNumber) || !string.IsNullOrEmpty(cofvm.Filter))
            {
                query = query.Where(co => co.Title.Contains(cofvm.Filter) || co.Price.ToString().Contains(filterNumber.ToString()) || co.Duration.ToString().Contains(filterNumber.ToString()) || co.StatusType.Contains(cofvm.Filter));
            }

            // Final projection and ordering
            var orders = query
                .OrderBy(co => co.Status)
                .Select(co => new CompanyOrdersFilterOutputViewModel
                {
                    OrderId = co.OrderId,
                    OrderNumber = co.OrderNumber,
                    Title = co.Title,
                    Price = co.Price,
                    OrderDate = co.OrderDate.ToString("yyyy年MM月dd日 HH點mm分"),
                    PayDate = co.Status ? co.PayDate.Value.ToString("yyyy年MM月dd日") : "無",
                    Duration = co.Duration,
                    Status = co.Status,
                    StatusType = co.StatusType,
                })
                .OrderByDescending(co => co.OrderNumber);

            return orders;
        }

        //POST: Companies/CompanyOrders/GetDeadline
        [HttpPost]
        public async Task<string?> GetDeadline()
        {
            // 從 claims 中取得 CompanyId
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return null; // 處理未授權訪問的情況
            }
            int companyId = int.Parse(CompanyId);

            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
            {
                return string.Empty;
            }

            var companyDeadline = company.Deadline;
            return companyDeadline.HasValue ? companyDeadline.Value.ToString("yyyy年MM月dd日 HH點mm分") : null;
        }

        //POST: Companies/CompanyOrders/CancelOrder
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> CancelOrder([FromBody][Bind("OrderId")] CancelOrderViewModel covm)
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
