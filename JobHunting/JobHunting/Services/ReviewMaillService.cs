using System.Net.Mail;
using System.Net;
using System.Text;

namespace JobHunting.Services
{
    public class ReviewMaillService
    {
        public void SendEmail(string receiveMail, string subject,string ContactName,string CompanyName)
        {
            //驗證連結，會調用HomeController的VerifyEmail方法來判斷並修改驗證狀態
            string verifyUrl = $"https://duckjobhunting.azurewebsites.net/Home/VerifyStatusEmail";

            // 建立郵件內容
            string emailBody = $@"
                <html>
                <body>
                    <h3>親愛的 [{ContactName}先生/小姐]</h3>
                    <p>
                    感謝您提交的申請。我們很高興通知您，經過仔細審核，貴公司 <span style='color:red'>({CompanyName})</span> 的申請已順利通過！
                    接下來，您可以登入我們的平台，並根據後續指引進行操作。如有任何問題或需要協助，請隨時與我們聯繫。
                    再次感謝您對我們的信任，期待您在平台上的更多參與。</p>
                    <p><a href='{verifyUrl}'>點擊這裡登入</a></p>
                    <br/>
                    <p>如果您無法點擊連結，請將以下網址複製到瀏覽器中打開：</p>
                    <p>{verifyUrl}</p>
                    <br/>
                    
                    <p>此為系統自動發送的郵件，請勿回覆。</p>
                    <br/>
                    <footer>[小鴨上工人力有限公司]祝您愉快</footer>
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
