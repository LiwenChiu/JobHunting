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
using System.Security.Policy;
using Newtonsoft.Json;
using NuGet.Protocol;
using Microsoft.IdentityModel.Tokens;

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
                NewebPayStatus = co.NewebPayStatus,
            });

            // Apply numeric filters dynamically
            if (int.TryParse(cofvm.Filter, out int filterNumber) || !string.IsNullOrEmpty(cofvm.Filter))
            {
                query = query.Where(co => co.Title.Contains(cofvm.Filter) || co.Price.ToString().Contains(filterNumber.ToString()) || co.Duration.ToString().Contains(filterNumber.ToString()) || co.StatusType.Contains(cofvm.Filter));
            }

            // Final projection and ordering
            var orders = query
                //.OrderBy(co => co.Status)
                .OrderByDescending(co => co.OrderDate)
                //.ThenBy(co => co.NewebPayStatus)
                .Select(co => new CompanyOrdersFilterOutputViewModel
                {
                    OrderId = co.OrderId,
                    OrderNumber = co.OrderNumber,
                    Title = co.Title,
                    Price = co.Price,
                    OrderDate = co.OrderDate.ToString("yyyy年MM月dd日 HH點mm分"),
                    PayDate = co.Status ? co.PayDate.Value.ToString("yyyy年MM月dd日 HH點mm分") : "無",
                    Duration = co.Duration,
                    Status = co.Status,
                    StatusType = co.StatusType,
                });


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
        public async Task<SendToNewebPaySearchOutputVueViewModel> SendToNewebPaySearch([FromBody][Bind("OrderId")] SendToNewebPaySearchInViewModel inModel)
        {
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(CompanyId))
            {
                return new SendToNewebPaySearchOutputVueViewModel
                {
                    Status = false,
                };
            }

            if (!ModelState.IsValid)
            {
                return new SendToNewebPaySearchOutputVueViewModel
                {
                    Status = false,
                };
            }

            int companyId = int.Parse(CompanyId);

            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
            {
                return new SendToNewebPaySearchOutputVueViewModel
                {
                    Status = false,
                };
            }

            var companyOrder = await _context.CompanyOrders.FindAsync(inModel.OrderId);
            if (companyOrder == null || companyOrder.CompanyId != companyId)
            {
                return new SendToNewebPaySearchOutputVueViewModel
                {
                    Status = false,
                };
            }

            if (companyOrder.Status)
            {
                var returnData = new SendToNewebPaySearchOutputVueViewModel
                {
                    Status = true,
                    OrderData = new SendToNewebPaySearchOutputCompanyViewModel
                    {
                        OrderId = companyOrder.OrderId,
                        Price = decimal.ToInt32(companyOrder.Price),
                        Title = companyOrder.Title,
                        Orderdate = companyOrder.OrderDate.ToString("yyyy年MM月dd日 HH點mm分"),
                        PayDate = companyOrder.PayDate.HasValue ? companyOrder.PayDate.Value.ToString("yyyy年MM月dd日 HH點mm分") : null,
                        Duration = companyOrder.Duration,
                        StatusType = companyOrder.StatusType,
                        TradeNo = companyOrder.TradeNo.IsNullOrEmpty() ? null : companyOrder.TradeNo,
                        PaymentType = companyOrder.PaymentType.IsNullOrEmpty() ? null : companyOrder.PaymentType,
                    },
                };

                return returnData;
            }

            SendToNewebPaySearchOutViewModel outModel = new SendToNewebPaySearchOutViewModel();

            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var MerchantID = Config.GetSection("MerchantID").Value;
            int Price = decimal.ToInt32(companyOrder.Price);

            List<KeyValuePair<string, string>> CheckValue = new List<KeyValuePair<string, string>>();
            CheckValue.Add(new KeyValuePair<string, string>("Amt", Price.ToString()));
            CheckValue.Add(new KeyValuePair<string, string>("MerchantID", MerchantID));
            CheckValue.Add(new KeyValuePair<string, string>("MerchantOrderNo", companyOrder.OrderId));

            string CheckValueParam = string.Join("&", CheckValue.Select(x => $"{x.Key}={x.Value}"));

            outModel.MerchantID = MerchantID;
            outModel.Version = "1.3";
            outModel.RespondType = "JSON";

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

            var outModelReturn = SearchPostFormDataAsync(outModel).Result;
            var outModelReturnResult = outModelReturn.Result;
            if(outModelReturnResult == null)
            {
                return new SendToNewebPaySearchOutputVueViewModel
                {
                    Status = false,
                };
            }
            string TradeStatus = outModelReturnResult.TradeStatus;

            if(outModelReturnResult.MerchantID != MerchantID || outModelReturnResult.MerchantOrderNo != companyOrder.OrderId)
            {
                return new SendToNewebPaySearchOutputVueViewModel
                {
                    Status = false,
                };
            }

            var StatusType = TradeStatus switch
            {
                "0" => "尚未付款",
                "1" => "付款成功",
                "2" => "付款失敗",
                "3" => "已取消",
                "6" => "退款",
                _ => "錯誤",
            };

            if(companyOrder.StatusType == StatusType || TradeStatus == "0")
            {
                return new SendToNewebPaySearchOutputVueViewModel
                {
                    Status = true,
                    OrderData = new SendToNewebPaySearchOutputCompanyViewModel
                    {
                        OrderId = companyOrder.OrderId,
                        Price = decimal.ToInt32(companyOrder.Price),
                        Title = companyOrder.Title,
                        Orderdate = companyOrder.OrderDate.ToString("yyyy年MM月dd日 HH點mm分"),
                        PayDate = companyOrder.PayDate.Value.ToString("yyyy年MM月dd日 HH點mm分"),
                        Duration = companyOrder.Duration,
                        StatusType = StatusType,
                        TradeNo = outModelReturnResult.TradeNo,
                        PaymentType = outModelReturnResult.PaymentType,
                    },
                };
            }

            if(TradeStatus == "1" || TradeStatus == "2" || TradeStatus == "3" || TradeStatus == "6")
            {
                companyOrder.Status = true;
                companyOrder.StatusType = StatusType;
                companyOrder.NewebPayStatus = outModelReturn.Status;
                companyOrder.NewebPayMessage = outModelReturn.Message;
                companyOrder.TradeNo = outModelReturnResult.TradeNo;
                companyOrder.PaymentType = outModelReturnResult.PaymentType;
                if(TradeStatus == "1")
                {
                    companyOrder.PayDate = DateTime.Parse(outModelReturnResult.PayTime);
                }

                _context.Entry(companyOrder).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException ex)
                {
                    return new SendToNewebPaySearchOutputVueViewModel
                    {
                        Status = false,
                    };
                }
            }

            return new SendToNewebPaySearchOutputVueViewModel
            {
                Status = false,
            };
        }

        // Search訂單 發送 form-data 的 POST 請求
        private static async Task<TradeInfoResponse?> SearchPostFormDataAsync(SendToNewebPaySearchOutViewModel outModel)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Set up form data
                    var formData = new Dictionary<string, string>
            {
                { "MerchantID", outModel.MerchantID },
                { "Version", outModel.Version },
                { "RespondType", outModel.RespondType },
                { "CheckValue", outModel.CheckValue },
                { "TimeStamp", outModel.TimeStamp },
                { "MerchantOrderNo", outModel.MerchantOrderNo },
                { "Amt", outModel.Amt.ToString() },
            };

                    var content = new FormUrlEncodedContent(formData);

                    // Send POST request
                    HttpResponseMessage response = await client.PostAsync("https://ccore.newebpay.com/API/QueryTradeInfo", content);
                    //HttpResponseMessage response = await client.PostAsync("https://core.newebpay.com/API/QueryTradeInfo", content);
                    response.EnsureSuccessStatusCode();

                    // Read response content as string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON response into TradeInfoResponse object
                    TradeInfoResponse? tradeInfoResponse = JsonConvert.DeserializeObject<TradeInfoResponse>(responseBody);

                    return tradeInfoResponse;
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Handle request errors
                Console.WriteLine($"Request error: {httpEx.Message}");
                return null;
            }
            catch (JsonException jsonEx)
            {
                // Handle JSON deserialization errors
                Console.WriteLine($"JSON error: {jsonEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Handle other potential exceptions
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }

        public class TradeInfoResponse
        {
            public string? Status { get; set; }
            public string? Message { get; set; }
            public Result Result { get; set; }
        }

        public class Result
        {
            public string? MerchantID { get; set; }

            public int Amt { get; set; }

            public string? TradeNo { get; set; }

            public string? MerchantOrderNo { get; set; }

            public string? TradeStatus { get; set; }

            public string? PaymentType { get; set; }

            public string? PayTime { get; set; }
        }

        /// <summary>
        /// 傳送取消授權要求至藍新金流
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        //POST: Companies/CompanyOrders/SendToNewebPayCancel
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToNewebPayCancel([FromBody][Bind("OrderId")] SendToNewebPayCancelInViewModel inModel)
        {
            string[] returnStatus = new string[2];
            var companyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(companyId))
            {
                return Json(new { Status = false, message = "失敗" });
            }

            int parsedCompanyId;
            if (!int.TryParse(companyId, out parsedCompanyId))
            {
                return Json(new { Status = false, message = "失敗" });
            }
            if (!ModelState.IsValid)
            {
                return Json(new { Status = false, message = "失敗" });
            }

            var order = await _context.CompanyOrders.FindAsync(inModel.OrderId);
            if (order == null || order.CompanyId != parsedCompanyId)
            {
                return Json(new { Status = false, message = "訂單不存在" });
            }

            if(order.PayDate.HasValue)
            {
                return Json(new { Status = false, message = "訂單取消失敗" });
            }

            List<KeyValuePair<string, string>> PostData_ = new List<KeyValuePair<string, string>>();
            PostData_.Add(new KeyValuePair<string, string>("RespondType", "JSON"));
            PostData_.Add(new KeyValuePair<string, string>("Version", "1.0"));
            PostData_.Add(new KeyValuePair<string, string>("Amt", order.Price.ToString()));
            PostData_.Add(new KeyValuePair<string, string>("MerchantOrderNo", order.OrderId));
            PostData_.Add(new KeyValuePair<string, string>("IndexType", "1"));
            PostData_.Add(new KeyValuePair<string, string>("TimeStamp", DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0)).ToUnixTimeSeconds().ToString()));

            string PostData_Param = string.Join("&", PostData_.Select(x => $"{x.Key}={x.Value}"));


            SendToNewebPayCancelOutViewModel outModel = new SendToNewebPayCancelOutViewModel();

            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string MerchantID = Config.GetSection("MerchantID").Value;
            string HashKey = Config.GetSection("HashKey").Value;//API 串接金鑰
            string HashIV = Config.GetSection("HashIV").Value;//API 串接密碼
            string PostData_Encrypt = EncryptAESHex(PostData_Param, HashKey, HashIV);

            outModel.MerchantID_ = MerchantID;
            outModel.PostData_ = PostData_Encrypt;

            var outModelReturn = CancelPostFormDataAsync(outModel).Result;
            var outModelReturnResult = outModelReturn.Result;
            if(outModelReturnResult == null)
            {
                return Json(new { Status = false, message = "訂單取消失敗" });
            }

            if(outModelReturn.Status != "SUCCESS" || outModelReturnResult.MerchantID != MerchantID || outModelReturnResult.MerchantOrderNo != order.OrderId)
            {
                return Json(new { Status = false, message = "訂單取消失敗" });
            }

            order.Status = true;
            order.StatusType = "已取消";

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return Json(new { Status = false, message = "訂單取消失敗" });
            }

            return Json(new { Status = true, message = "取消訂單成功" });

        }

        // Cancel訂單 發送 form-data 的 POST 請求
        private static async Task<TradeInfoCancelResponse> CancelPostFormDataAsync(SendToNewebPayCancelOutViewModel outModel)
        {
            using (HttpClient client = new HttpClient())
            {
                // 設置 form-data
                var formData = new Dictionary<string, string>
            {
                { "MerchantID_", outModel.MerchantID_ },
                { "PostData_", outModel.PostData_ },
            };

                var content = new FormUrlEncodedContent(formData);

                // 發送 POST 請求並接收回應
                HttpResponseMessage response = await client.PostAsync("https://ccore.newebpay.com/API/CreditCard/Cancel", content);
                //HttpResponseMessage response = await client.PostAsync("https://core.newebpay.com/API/CreditCard/Cancel", content);
                response.EnsureSuccessStatusCode();

                // 讀取回應內容為字符串
                string responseBody = await response.Content.ReadAsStringAsync();

                // 將回應的 JSON 反序列化為 TradeInfoResponse 類型
                TradeInfoCancelResponse tradeInfoCancelResponse = JsonConvert.DeserializeObject<TradeInfoCancelResponse>(responseBody);

                return tradeInfoCancelResponse;
            }
        }

        public class TradeInfoCancelResponse
        {
            public string Status { get; set; }

            public string Message { get; set; }

            public CancelResult Result { get; set; }
        }

        public class CancelResult
        {
            public string MerchantID { get; set; }

            public string TradeNo { get; set; }

            public int Amt {  get; set; }

            public string MerchantOrderNo { get; set; }

            public string CheckCode { get; set; }
        }

        /// <summary>
        /// 加密後再轉 16 進制字串
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <param name="cryptoKey">加密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        /// <returns>加密後的字串</returns>
        public string EncryptAESHex(string source, string cryptoKey, string cryptoIV)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(source))
            {
                var encryptValue = EncryptAES(Encoding.UTF8.GetBytes(source), cryptoKey, cryptoIV);

                if (encryptValue != null)
                {
                    result = BitConverter.ToString(encryptValue)?.Replace("-", string.Empty)?.ToLower();
                }
            }

            return result;
        }

        /// <summary>
        /// 字串加密AES
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <param name="cryptoKey">加密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        /// <returns>加密後字串</returns>
        public byte[] EncryptAES(byte[] source, string cryptoKey, string cryptoIV)
        {
            byte[] dataKey = Encoding.UTF8.GetBytes(cryptoKey);
            byte[] dataIV = Encoding.UTF8.GetBytes(cryptoIV);

            using (var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Mode = System.Security.Cryptography.CipherMode.CBC;
                aes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                aes.Key = dataKey;
                aes.IV = dataIV;

                using (var encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(source, 0, source.Length);
                }
            }
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
    }
}
