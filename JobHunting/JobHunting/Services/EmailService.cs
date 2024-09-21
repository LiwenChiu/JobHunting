﻿using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace JobHunting.Services
{

    public class EmailService
    {
        //private readonly IMemoryCache _cache;

        //public  EmailService(IMemoryCache cache)
        //{
        //    _cache = cache;
        //}

        // 根據傳回的email生成驗證token並編碼成base64將驗證連結直接傳到mail裡
        public string GenerateVerificationToken(string email)
        {
            //token = guid唯一碼
            string token = Guid.NewGuid().ToString();
            DateTime expiry = DateTime.UtcNow.AddHours(1);

            //base64編碼
            string encodedEmail = Convert.ToBase64String(Encoding.UTF8.GetBytes(email));
            string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            string verificationLink = $"https://localhost:7169/Home/VerifyEmail?token={encodedToken}&email={encodedEmail}&expiry={expiry.Ticks}";

            return verificationLink;
        }




        public void SendEmail(string receiveMail, string subject)
        {
            //驗證連結，會調用HomeController的VerifyEmail方法來判斷並修改驗證狀態
            string verifyUrl = GenerateVerificationToken(receiveMail);

            // 建立郵件內容
            string emailBody = $@"
                <html>
                <body>
                    <h1>感謝您註冊！</h1>
                    <p>請點擊下方連結以完成您的電子郵件驗證：</p>
                    <p><a href='{verifyUrl}'>點擊這裡驗證</a></p>
                    <p>如果您無法點擊連結，請將以下網址複製到瀏覽器中打開：</p>
                    <p>{verifyUrl}</p>
                    <br/>
                    <p>此為系統自動發送的郵件，請勿回覆。</p>
                </body>
                </html>";




            // 使用 Google Mail Server 發信
            string GoogleID = "TIM102FirstGroup@gmail.com"; //Google 發信帳號
            string TempPwd = "mgrc fdks oypm sewa"; //應用程式密碼
            //string ReceiveMail = "TIM102FirstGroup@gmail.com"; //接收信箱

            string SmtpServer = "smtp.gmail.com";
            int SmtpPort = 587;
            MailMessage mms = new MailMessage();
            mms.From = new MailAddress(GoogleID);
            mms.Subject = subject; //信件主題
            mms.Body = emailBody; //信件內容
            mms.IsBodyHtml = true;
            mms.SubjectEncoding = Encoding.UTF8;
            mms.To.Add(new MailAddress(receiveMail));
            using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(GoogleID, TempPwd);//寄信帳密 
                client.Send(mms); //寄出信件
            }
        }
    }
}
