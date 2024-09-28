using JobHunting.Areas.Companies.ViewModel;
using JobHunting.Models;
using JobHunting.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace JobHunting.Services
{
    public class NewebPaySearchNonCurrentService
    {
        private readonly DuckContext _context;

        public NewebPaySearchNonCurrentService(DuckContext context)
        {
            _context = context;
        }

        public async Task NewebPaySearchStatusFalse()
        {
            var taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            var nowTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taiwanTimeZone);

            var companyOrders = _context.CompanyOrders.Where(co => co.ExpireDate.HasValue && !co.PayDate.HasValue).Where(co => DateTime.Compare((DateTime)co.ExpireDate, nowTime) < 0);

            if (!companyOrders.IsNullOrEmpty())
            {
                foreach (var companyOrder in companyOrders)
                {
                    await SendToNewebPaySearchNonCurrentRecurring(companyOrder);
                }
            }

        }

        /// <summary>
        /// 傳送查詢要求至藍新金流
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        ///POST: Companies/CompanyOrders/SendToNewebPaySearch
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task SendToNewebPaySearchNonCurrentRecurring(CompanyOrder companyOrder)
        {
            SendToNewebPaySearchRecurringOutViewModel outModel = new SendToNewebPaySearchRecurringOutViewModel();

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
            if (outModelReturn.Status != "SUCCESS")
            {
                companyOrder.Status = true;
                companyOrder.StatusType = "付款失敗";
                companyOrder.ExpireDate = null;

                _context.Entry(companyOrder).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return;
                }

                return;
            }

            var outModelReturnResult = outModelReturn.Result;
            if (outModelReturnResult == null)
            {
                return;
            }
            string TradeStatus = outModelReturnResult.TradeStatus;

            if (outModelReturnResult.MerchantID != MerchantID || outModelReturnResult.MerchantOrderNo != companyOrder.OrderId)
            {
                return;
            }

            var StatusType = TradeStatus switch
            {
                string type when type == "0" => "尚未付款",
                string type when type == "1" => "付款成功",
                string type when type == "2" => "付款失敗",
                string type when type == "3" => "已取消",
                string type when type == "6" => "退款",
                _ => "錯誤",
            };

            if (TradeStatus != "1")
            {
                companyOrder.StatusType = StatusType;
                companyOrder.ExpireDate = null;
                companyOrder.Status = true;

                _context.Entry(companyOrder).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return;
                }

                return;
            }
            else
            {
                companyOrder.Status = true;
                companyOrder.ExpireDate = null;
                companyOrder.StatusType = StatusType;
                companyOrder.NewebPayStatus = outModelReturn.Status;
                companyOrder.NewebPayMessage = outModelReturn.Message;

                _context.Entry(companyOrder).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return;
                }
            }

            return;
        }

        // Search訂單 發送 form-data 的 POST 請求
        private static async Task<TradeInfoResponse> SearchPostFormDataAsync(SendToNewebPaySearchRecurringOutViewModel outModel)
        {
            using (HttpClient client = new HttpClient())
            {
                // 設置 form-data
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

                // 發送 POST 請求並接收回應
                HttpResponseMessage response = await client.PostAsync("https://ccore.newebpay.com/API/QueryTradeInfo", content);
                //HttpResponseMessage response = await client.PostAsync("https://core.newebpay.com/API/QueryTradeInfo", content);
                response.EnsureSuccessStatusCode();

                // 讀取回應內容為字符串
                string responseBody = await response.Content.ReadAsStringAsync();

                // 將回應的 JSON 反序列化為 TradeInfoResponse 類型
                TradeInfoResponse tradeInfoResponse = JsonConvert.DeserializeObject<TradeInfoResponse>(responseBody);

                return tradeInfoResponse;
            }
        }

        public class TradeInfoResponse
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public Result Result { get; set; }
        }

        public class Result
        {
            public string MerchantID { get; set; }

            public int Amt { get; set; }

            public string TradeNo { get; set; }

            public string MerchantOrderNo { get; set; }

            public string TradeStatus { get; set; }

            public string PaymentType { get; set; }

            public DateTime? PayTime { get; set; }
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
