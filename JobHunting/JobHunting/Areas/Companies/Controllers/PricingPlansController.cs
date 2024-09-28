using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using System.Numerics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class PricingPlansController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public PricingPlansController(DuckCompaniesContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "company")]
        // GET: Companies/PricingPlans
        public async Task<IActionResult> Index()
        {
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(CompanyId))
            {
                return Json(new { message = "未授權訪問" });
            }

            return View();
        }

        [Authorize(Roles = "company")]
        // POST: Companies/PricingPlans/BootFilterPage
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<PricingPlanOutputViewModel>> BootFilterPage([FromBody][Bind("PlanId,Title,Intro,Duration,Price,Discount,Status")] PricingPlanViewModel ppvm)
        {
            return _context.PricingPlans.Select(pp => new PricingPlanViewModel
            {
                PlanId = pp.PlanId,
                Title = pp.Title,
                Intro = pp.Intro,
                Duration = pp.Duration,
                Price = pp.Price,
                Discount = pp.Discount,
                Status = pp.Status
            }).Where(ppvmfilter => (ppvmfilter.Title.Contains(ppvm.Title) ||
                                    ppvmfilter.Intro.Contains(ppvm.Intro) ||
                                    ppvmfilter.Duration.ToString().Contains(ppvm.Duration.ToString()) ||
                                    ppvmfilter.Price.ToString().Contains(ppvm.Price.ToString()) ||
                                    ppvmfilter.Discount.ToString().Contains(ppvm.Discount.ToString())) && ppvmfilter.Status == true)
            .Select(pp => new PricingPlanOutputViewModel
            {
                PlanId = pp.PlanId,
                Title = pp.Title,
                Intro = pp.Intro,
                Duration = pp.Duration,
                Price = pp.Price,
                Discount = pp.Discount
            });
        }

        /// <summary>
        /// 傳送訂單至藍新金流
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToNewebPay([FromBody][Bind("PlanId")] SendToNewebPayInViewModel inModel)
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

            int orderNumber = company.OrderCount + 1;

            var taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            var nowTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taiwanTimeZone);
            var ExpirationTime = nowTime.AddDays(3);
            string ExpirationTimeStr = ExpirationTime.ToString("yyyyMMdd");

            string companyIdZeroStr = companyId.ToString("X7");
            string companyOrderCountStr = orderNumber.ToString("X7");

            var MerchantOrderNo = $"{companyIdZeroStr}" + $"{companyOrderCountStr}" + nowTime.ToString("yyyyMMdd");

            var pricingPlan = await _context.PricingPlans.FindAsync(inModel.PlanId);
            if (pricingPlan == null || !pricingPlan.Status)
            {
                return Json(new { message = "方案不存在" });
            }

            var Amt = (pricingPlan.Price) * (pricingPlan.Discount);
            string? AmtStr = Decimal.ToInt32((decimal)Amt).ToString();

            SendToNewebPayOutViewModel outModel = new SendToNewebPayOutViewModel();

            // 藍新金流線上付款

            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var MerchantID = Config.GetSection("MerchantID").Value;

            string ReturnURL = "https://duckjobhunting.azurewebsites.net/Companies/PricingPlans/CallbackReturn"; //支付完成返回商店網址
            string CustomerURL = "https://duckjobhunting.azurewebsites.net/Companies/PricingPlans/CallbackCustomer"; //商店取號網址
            string NotifyURL = "https://duckjobhunting.azurewebsites.net/Companies/PricingPlans/CallbackNotify"; //支付通知網址
            string ClientBackURL = "https://duckjobhunting.azurewebsites.net/Companies/PricingPlans/Index"; //返回商店網址

            //交易欄位
            List<KeyValuePair<string, string>> TradeInfo = new List<KeyValuePair<string, string>>();
            // 商店代號
            TradeInfo.Add(new KeyValuePair<string, string>("MerchantID", MerchantID));
            // 回傳格式
            TradeInfo.Add(new KeyValuePair<string, string>("RespondType", "String"));
            // TimeStamp
            TradeInfo.Add(new KeyValuePair<string, string>("TimeStamp", DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0)).ToUnixTimeSeconds().ToString()));
            // 串接程式版本
            TradeInfo.Add(new KeyValuePair<string, string>("Version", "2.0"));
            // 商店訂單編號
            TradeInfo.Add(new KeyValuePair<string, string>("MerchantOrderNo", MerchantOrderNo));
            // 訂單金額
            TradeInfo.Add(new KeyValuePair<string, string>("Amt", AmtStr));
            // 商品資訊
            TradeInfo.Add(new KeyValuePair<string, string>("ItemDesc", pricingPlan.Title));
            // 交易有效時間
            TradeInfo.Add(new KeyValuePair<string, string>("TradeLimit", "900"));
            // 繳費有效期限(適用於非即時交易)
            TradeInfo.Add(new KeyValuePair<string, string>("ExpireDate", ExpirationTimeStr));
            // 支付完成返回商店網址
            TradeInfo.Add(new KeyValuePair<string, string>("ReturnURL", ReturnURL));
            // 支付通知網址
            TradeInfo.Add(new KeyValuePair<string, string>("NotifyURL", NotifyURL));
            // 商店取號網址
            TradeInfo.Add(new KeyValuePair<string, string>("CustomerURL", CustomerURL));
            // 支付取消返回商店網址
            TradeInfo.Add(new KeyValuePair<string, string>("ClientBackURL", ClientBackURL));
            // 付款人電子信箱
            TradeInfo.Add(new KeyValuePair<string, string>("Email", company.ContactEmail));
            //信用卡 付款
            TradeInfo.Add(new KeyValuePair<string, string>("CREDIT", "1"));
            //Apple Pay 付款
            TradeInfo.Add(new KeyValuePair<string, string>("APPLEPAY", "1"));
            //Google Pay 付款
            TradeInfo.Add(new KeyValuePair<string, string>("ANDROIDPAY", "1"));
            //LINE Pay 付款
            //TradeInfo.Add(new KeyValuePair<string, string>("LINEPAY", "1"));
            //WEBATM 付款
            TradeInfo.Add(new KeyValuePair<string, string>("WEBATM", "1"));
            //ATM 付款
            TradeInfo.Add(new KeyValuePair<string, string>("VACC", "1"));
            //超商代碼 付款
            TradeInfo.Add(new KeyValuePair<string, string>("CVS", "1"));
            //超商條碼繳費 付款
            TradeInfo.Add(new KeyValuePair<string, string>("BARCODE", "1"));
            //玉山Wallet 付款
            TradeInfo.Add(new KeyValuePair<string, string>("ESUNWALLET", "1"));
            //台灣Pay 付款
            TradeInfo.Add(new KeyValuePair<string, string>("TAIWANPAY", "1"));

            string TradeInfoParam = string.Join("&", TradeInfo.Select(x => $"{x.Key}={x.Value}"));

            // API 傳送欄位
            // 商店代號
            outModel.MerchantID = MerchantID;
            // 串接程式版本
            outModel.Version = "2.0";
            //交易資料 AES 加解密
            string HashKey = Config.GetSection("HashKey").Value;//API 串接金鑰
            string HashIV = Config.GetSection("HashIV").Value;//API 串接密碼
            string TradeInfoEncrypt = EncryptAESHex(TradeInfoParam, HashKey, HashIV);
            outModel.TradeInfo = TradeInfoEncrypt;
            //交易資料 SHA256 加密
            outModel.TradeSha = EncryptSHA256($"HashKey={HashKey}&{TradeInfoEncrypt}&HashIV={HashIV}");

            CompanyOrder companyOrder = new CompanyOrder
            {
                OrderId = MerchantOrderNo,
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                GUINumber = company.GUINumber,
                PlanId = pricingPlan.PlanId,
                OrderNumber = orderNumber,
                Title = pricingPlan.Title,
                Price = Amt,
                OrderDate = nowTime,
                Duration = pricingPlan.Duration,
                Status = false,
                StatusType = "尚未付款",
            };

            company.OrderCount++;

            _context.Entry(company).State = EntityState.Modified;

            _context.CompanyOrders.Add(companyOrder);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return Json(new { message = "失敗" });
            }

            return Json(outModel);
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

        /// <summary>
        /// 支付完成返回網址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CallbackReturn()
        {
            string NewebPayStatus = "";
            // 付款失敗跳離執行
            foreach (var item in Request.Form)
            {
                if (item.Key == "Status")
                {
                    NewebPayStatus = item.Value;
                }
            }

            // 解密訊息
            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string HashKey = Config.GetSection("HashKey").Value;//API 串接金鑰
            string HashIV = Config.GetSection("HashIV").Value;//API 串接密碼
            string TradeInfoDecrypt = DecryptAESHex(Request.Form["TradeInfo"], HashKey, HashIV);
            NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(TradeInfoDecrypt);

            // 接收TradeInfo參數
            NewebPayReturnTradeInfoViewModel result = new NewebPayReturnTradeInfoViewModel();
            result.Status = NewebPayStatus;
            foreach (String key in decryptTradeCollection.AllKeys)
            {
                if (key == "Status" && decryptTradeCollection[key] != NewebPayStatus)
                {
                    return BadRequest();
                }
                if (key == "Message")
                {
                    result.Message = decryptTradeCollection[key];
                }
                if (key == "MerchantID")
                {
                    result.MerchantID = decryptTradeCollection[key];
                }
                if (key == "Amt")
                {
                    result.Amt = decryptTradeCollection[key];
                }
                if (key == "TradeNo")
                {
                    result.TradeNo = decryptTradeCollection[key];
                }
                if (key == "MerchantOrderNo")
                {
                    result.MerchantOrderNo = decryptTradeCollection[key];
                }
                if (key == "PaymentType")
                {
                    result.PaymentType = decryptTradeCollection[key];
                }
                if (NewebPayStatus == "SUCCESS" && key == "PayTime")
                {
                    result.PayTime = DateTime.Parse(decryptTradeCollection[key]);
                }
                else if (NewebPayStatus != "SUCCESS" && key == "PayTime")
                {
                    result.PayTime = null;
                }
                if (key == "IP")
                {
                    result.IP = decryptTradeCollection[key];
                }
                if (key == "EscrowBank")
                {
                    result.EscrowBank = decryptTradeCollection[key];
                }
            }

            var companyOrder = await _context.CompanyOrders.FindAsync(result.MerchantOrderNo);
            if (companyOrder == null)
            {
                return NotFound();
            }

            if (NewebPayStatus == "SUCCESS")
            {
                companyOrder.StatusType = "付款成功";
            }
            else
            {
                companyOrder.Status = true;
                companyOrder.StatusType = "付款失敗";
                companyOrder.ExpireDate = null;

                _context.Entry(companyOrder).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest();
                }

                return BadRequest();
            }

            companyOrder.NewebPayStatus = NewebPayStatus;
            companyOrder.NewebPayMessage = result.Message;
            companyOrder.TradeNo = result.TradeNo;
            companyOrder.PaymentType = result.PaymentType;
            companyOrder.PayDate = result.PayTime;
            companyOrder.ExpireDate = null;
            companyOrder.IP = result.IP;
            companyOrder.EscrowBank = result.EscrowBank;

            var company = await _context.Companies.FindAsync(companyOrder.CompanyId);
            if (company == null) { return NotFound(); }

            var taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            var nowTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taiwanTimeZone);
            DateTime deadline = (DateTime)(company.Deadline.HasValue ? company.Deadline : nowTime);
            deadline = deadline.AddDays(companyOrder.Duration);
            company.Deadline = deadline;
            companyOrder.Status = true;

            _context.Entry(company).State = EntityState.Modified;
            _context.Entry(companyOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }

            ViewData["CallbackReturnResult"] = result;

            return View();
        }

        /// <summary>
        /// 16 進制字串解密
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <param name="cryptoKey">加密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        /// <returns>解密後的字串</returns>
        public string DecryptAESHex(string source, string cryptoKey, string cryptoIV)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(source))
            {
                // 將 16 進制字串 轉為 byte[] 後
                byte[] sourceBytes = ToByteArray(source);

                if (sourceBytes != null)
                {
                    // 使用金鑰解密後，轉回 加密前 value
                    result = Encoding.UTF8.GetString(DecryptAES(sourceBytes, cryptoKey, cryptoIV)).Trim();
                }
            }

            return result;
        }

        /// <summary>
        /// 將16進位字串轉換為byteArray
        /// </summary>
        /// <param name="source">欲轉換之字串</param>
        /// <returns></returns>
        public byte[] ToByteArray(string source)
        {
            byte[] result = null;

            if (!string.IsNullOrWhiteSpace(source))
            {
                var outputLength = source.Length / 2;
                var output = new byte[outputLength];

                for (var i = 0; i < outputLength; i++)
                {
                    output[i] = Convert.ToByte(source.Substring(i * 2, 2), 16);
                }
                result = output;
            }

            return result;
        }

        /// <summary>
        /// 字串解密AES
        /// </summary>
        /// <param name="source">解密前字串</param>
        /// <param name="cryptoKey">解密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        /// <returns>解密後字串</returns>
        public static byte[] DecryptAES(byte[] source, string cryptoKey, string cryptoIV)
        {
            byte[] dataKey = Encoding.UTF8.GetBytes(cryptoKey);
            byte[] dataIV = Encoding.UTF8.GetBytes(cryptoIV);

            using (var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Mode = System.Security.Cryptography.CipherMode.CBC;
                // 智付通無法直接用PaddingMode.PKCS7，會跳"填補無效，而且無法移除。"
                // 所以改為PaddingMode.None並搭配RemovePKCS7Padding
                aes.Padding = System.Security.Cryptography.PaddingMode.None;
                aes.Key = dataKey;
                aes.IV = dataIV;

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] data = decryptor.TransformFinalBlock(source, 0, source.Length);
                    int iLength = data[data.Length - 1];
                    var output = new byte[data.Length - iLength];
                    Buffer.BlockCopy(data, 0, output, 0, output.Length);
                    return output;
                }
            }
        }

        /// <summary>
        /// 商店取號網址
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CallbackCustomer()
        {
            // 付款失敗跳離執行
            foreach (var item in Request.Form)
            {
                if (item.Key == "Status" && item.Value != "SUCCESS")
                {
                    return BadRequest();
                }
            }

            // 解密訊息
            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string HashKey = Config.GetSection("HashKey").Value;//API 串接金鑰
            string HashIV = Config.GetSection("HashIV").Value;//API 串接密碼
            string TradeInfoDecrypt = DecryptAESHex(Request.Form["TradeInfo"], HashKey, HashIV);
            NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(TradeInfoDecrypt);

            // 接收TradeInfo參數
            NewebPayTakeNumberTradeInfoViewModel result = new NewebPayTakeNumberTradeInfoViewModel();

            foreach (String key in decryptTradeCollection.AllKeys)
            {
                if (key == "Status" && decryptTradeCollection[key] == "SUCCESS")
                {
                    result.Status = decryptTradeCollection[key];
                }
                else if (key == "Status" && decryptTradeCollection[key] != "SUCCESS")
                {
                    return BadRequest();
                }
                if (key == "Message")
                {
                    result.Message = decryptTradeCollection[key];
                }
                if (key == "MerchantID")
                {
                    result.MerchantID = decryptTradeCollection[key];
                }
                if (key == "Amt")
                {
                    result.Amt = decryptTradeCollection[key];
                }
                if (key == "TradeNo")
                {
                    result.TradeNo = decryptTradeCollection[key];
                }
                if (key == "MerchantOrderNo")
                {
                    result.MerchantOrderNo = decryptTradeCollection[key];
                }
                if (key == "PaymentType")
                {
                    result.PaymentType = decryptTradeCollection[key];
                }
                if (key == "ExpireDate")
                {
                    result.ExpireDate = DateTime.Parse(decryptTradeCollection[key]);
                }
                if (result.PaymentType == "VACC" && key == "BankCode")
                {
                    result.BankCode = decryptTradeCollection[key];
                }
                if (result.PaymentType == "VACC" && key == "CodeNo")
                {
                    result.ATMCodeNo = decryptTradeCollection[key];
                }
                if (result.PaymentType == "CVS" && key == "CodeNo")
                {
                    result.CodeNo = decryptTradeCollection[key];
                }
                if (result.PaymentType == "CVS" && key == "ExpireTime")
                {
                    result.ExpireTime = TimeOnly.Parse(decryptTradeCollection[key]).ToString("HH點mm分ss秒");
                }
                if (result.PaymentType == "BARCODE" && key == "Barcode_1")
                {
                    result.Barcode_1 = decryptTradeCollection[key];
                }
                if (result.PaymentType == "BARCODE" && key == "Barcode_2")
                {
                    result.Barcode_2 = decryptTradeCollection[key];
                }
                if (result.PaymentType == "BARCODE" && key == "Barcode_3")
                {
                    result.Barcode_3 = decryptTradeCollection[key];
                }
            }

            var companyOrder = await _context.CompanyOrders.FindAsync(result.MerchantOrderNo);
            if (companyOrder == null)
            {
                return NotFound();
            }

            if (result.Status == "SUCCESS")
            {
                companyOrder.Status = true;
                companyOrder.StatusType = "取號完成";
                companyOrder.TradeNo = result.TradeNo;
                companyOrder.PaymentType = result.PaymentType;
                companyOrder.ExpireDate = result.ExpireDate;
            }

            var company = await _context.Companies.FindAsync(companyOrder.CompanyId);
            if (company == null) { return NotFound(); }

            _context.Entry(companyOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }

            ViewData["CallbackTakeNumberReturnResult"] = result;

            return View();
        }

        /// <summary>
        /// 支付通知網址
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CallbackNotify()
        {
            string NewebPayStatus = "";
            foreach (var item in Request.Form)
            {
                if (item.Key == "Status")
                {
                    NewebPayStatus = item.Value;
                    break;
                }
            }

            string MerchantOrderNo = "";

            // 解密訊息
            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string HashKey = Config.GetSection("HashKey").Value;//API 串接金鑰
            string HashIV = Config.GetSection("HashIV").Value;//API 串接密碼
            string TradeInfoDecrypt = DecryptAESHex(Request.Form["Result"], HashKey, HashIV);
            NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(TradeInfoDecrypt);
            foreach (String key in decryptTradeCollection.AllKeys)
            {
                if (key == "MerchantOrderNo")
                {
                    MerchantOrderNo = decryptTradeCollection[key];
                }
            }

            var companyOrder = await _context.CompanyOrders.FindAsync(MerchantOrderNo);
            if (companyOrder == null)
            {
                return NotFound();
            }

            if (companyOrder.Status)
            {
                return Ok();
            }

            // 接收TradeInfo參數
            NewebPayReturnTradeInfoViewModel result = new NewebPayReturnTradeInfoViewModel();
            result.Status = NewebPayStatus;
            result.MerchantOrderNo = MerchantOrderNo;
            foreach (String key in decryptTradeCollection.AllKeys)
            {
                if (key == "Status" && decryptTradeCollection[key] != NewebPayStatus)
                {
                    return BadRequest();
                }
                if (key == "Message")
                {
                    result.Message = decryptTradeCollection[key];
                }
                if (key == "MerchantID")
                {
                    result.MerchantID = decryptTradeCollection[key];
                }
                if (key == "Amt")
                {
                    result.Amt = decryptTradeCollection[key];
                }
                if (key == "TradeNo")
                {
                    result.TradeNo = decryptTradeCollection[key];
                }
                if (key == "PaymentType")
                {
                    result.PaymentType = decryptTradeCollection[key];
                }
                if (NewebPayStatus == "SUCCESS" && key == "PayTime")
                {
                    result.PayTime = DateTime.Parse(decryptTradeCollection[key]);
                }
                else if (NewebPayStatus != "SUCCESS" && key == "PayTime")
                {
                    result.PayTime = null;
                }
                if (key == "IP")
                {
                    result.IP = decryptTradeCollection[key];
                }
                if (key == "EscrowBank")
                {
                    result.EscrowBank = decryptTradeCollection[key];
                }
            }


            if (NewebPayStatus == "SUCCESS")
            {
                if(companyOrder.StatusType == "付款成功")
                {
                    return Ok();
                }
                companyOrder.StatusType = "付款成功";
            }
            else
            {
                companyOrder.Status = true;
                if (companyOrder.StatusType == "付款失敗")
                {
                    return BadRequest();
                }
                companyOrder.StatusType = "付款失敗";
                companyOrder.ExpireDate = null;

                _context.Entry(companyOrder).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest();
                }

                return BadRequest();
            }

            companyOrder.NewebPayStatus = NewebPayStatus;
            companyOrder.NewebPayMessage = result.Message;
            companyOrder.TradeNo = result.TradeNo;
            companyOrder.PaymentType = result.PaymentType;
            companyOrder.PayDate = result.PayTime;
            companyOrder.ExpireDate = null;
            companyOrder.IP = result.IP;
            companyOrder.EscrowBank = result.EscrowBank;

            var company = await _context.Companies.FindAsync(companyOrder.CompanyId);
            if (company == null) { return NotFound(); }

            var taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            var nowTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taiwanTimeZone);
            DateTime deadline = (DateTime)(company.Deadline.HasValue ? company.Deadline : nowTime);
            deadline = deadline.AddDays(companyOrder.Duration);
            company.Deadline = deadline;

            companyOrder.Status = true;
            _context.Entry(company).State = EntityState.Modified;
            _context.Entry(companyOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
