using System.Net.Mail;
using System.Net;
using System.Text;

namespace JobHunting.Services
{

    public class EmailService
    {
            public void SendEmail(string receiveMail, string subject, string body)
        {
            // 使用 Google Mail Server 發信
            string GoogleID = "TIM102FirstGroup@gmail.com"; //Google 發信帳號
            string TempPwd = "mgrc fdks oypm sewa"; //應用程式密碼
            //string ReceiveMail = "TIM102FirstGroup@gmail.com"; //接收信箱

            string SmtpServer = "smtp.gmail.com";
            int SmtpPort = 587;
            MailMessage mms = new MailMessage();
            mms.From = new MailAddress(GoogleID);
            mms.Subject = subject; //信件主題
            mms.Body = body; //信件內容
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
