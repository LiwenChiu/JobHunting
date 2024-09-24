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
using System.Text;

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
            return companyDeadline.HasValue ? companyDeadline.Value.ToString("yyyy年MM月dd日") : null;
        }

        /// <summary>
        /// 傳送查詢要求至藍新金流
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        ///POST: Companies/CompanyOrders/SendToNewebPaySearch
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToNewebPaySearch([FromBody][Bind("OrderId")] SendToNewebPaySearchInViewModel inModel)
        {
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(CompanyId))
            {
                return Json(new { message = "未授權訪問" });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { message = "失敗" });
            }

            int companyId = int.Parse(CompanyId);

            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
            {
                return Json(new { message = "失敗" });
            }

            var companyOrder = await _context.CompanyOrders.FindAsync(inModel.OrderId);
            if(companyOrder == null || companyOrder.CompanyId != companyId || !companyOrder.Status)
            {
                return Json(new { message = "訂單錯誤" });
            }

            SendToNewebPaySearchOutViewModel outModel = new SendToNewebPaySearchOutViewModel();

            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();

            var MerchantID = Config.GetSection("MerchantID").Value;
            int Price = decimal.ToInt32(companyOrder.Price);

            List<KeyValuePair<string,string>> CheckValue = new List<KeyValuePair<string,string>>();
            CheckValue.Add(new KeyValuePair<string, string>("Amt", Price.ToString()));
            CheckValue.Add(new KeyValuePair<string, string>("MerchantID", MerchantID));
            CheckValue.Add(new KeyValuePair<string, string>("MerchantOrderNo", companyOrder.OrderId));

            string CheckValueParam = string.Join("&", CheckValue.Select(x => $"{x.Key}={x.Value}"));

            outModel.MerchantID = MerchantID;
            outModel.Version = "1.3";
            outModel.RespondType = "String";

            string HashIV = Config.GetSection("HashIV").Value;//API 串接密碼
            string HashIVStr = $"IV={HashIV}";

            string HashKey = Config.GetSection("HashKey").Value;//API 串接金鑰
            string HashKeyStr = $"Key={HashKey}";

            string[] CheckValueList = [HashIVStr, CheckValueParam, HashKeyStr];

            string CheckValueStr = string.Join("&", CheckValueList);

            string CheckValueEncrypt = EncryptSHA256(CheckValueStr);

            outModel.CheckValue = CheckValueEncrypt;
            outModel.TimeStamp = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0)).ToUnixTimeSeconds().ToString();
            outModel.MerchantOrderNo = companyOrder.OrderId;
            outModel.Amt = Price;

            return Json(outModel);
        }

        /// <summary>
        /// 字串加密SHA256
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <returns>加密後字串</returns>
        public string EncryptSHA256(string source)
        {
            string result = string.Empty;

            using (System.Security.Cryptography.SHA256 algorithm = System.Security.Cryptography.SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(source));

                if (hash != null)
                {
                    result = BitConverter.ToString(hash)?.Replace("-", string.Empty)?.ToUpper();
                }

            }
            return result;
        }

        /// <summary>
        /// 傳送取消授權要求至藍新金流
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        //POST: Companies/CompanyOrders/SendToNewebPayCancel
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> SendToNewebPayCancel([FromBody][Bind("OrderId")] SendToNewebPayCancelInViewModel inModel)
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

            var order = await _context.CompanyOrders.FindAsync(inModel.OrderId);
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
