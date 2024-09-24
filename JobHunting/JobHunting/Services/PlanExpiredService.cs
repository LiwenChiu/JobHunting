using JobHunting.Models;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Services
{
    public class PlanExpiredService
    {
        private readonly DuckContext _context;
        private readonly PlanExpiredEmailService _emailService;

        public PlanExpiredService(DuckContext context, PlanExpiredEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // 發送到期前7天的提醒通知
        public async Task SendExpirationReminders()
        {
            var upcomingExpirationDate = DateTime.Now.AddDays(7);

            var companies = await _context.Companies
                .Where(c => c.Deadline.HasValue && c.Deadline.Value.Date == upcomingExpirationDate.Date)
                .ToListAsync();

            foreach (var company in companies)
            {
                var subject = "方案到期提醒";
                var body = $@"
                <html>
                <body>
                      <p>親愛的 {company.CompanyName},</p>
                      <p>您的方案將在 {company.Deadline.Value.ToShortDateString()} 到期。</p>
                      <p>請前往選擇新的方案，以免影響您的服務。</p>
                      <p><a href='https://localhost:7169/Companies/PricingPlans'>點擊這裡選擇方案</a></p>
                      <p>祝好，<br/>小鴨上工</p>
                </body>
                </html>";


                var toEmail = company.ContactEmail;

                await _emailService.SendEmailAsync(toEmail, subject, body);
                Console.WriteLine($"發送提醒給公司: {company.CompanyName}, 他們的方案將在 {company.Deadline.Value.ToShortDateString()} 到期.");
            }
        }

        // 關閉過期公司職缺
        public async Task CloseExpiredJobOpenings()
        {
            var now = DateTime.Now;

            // 查找已經到期的公司
            var expiredCompanies = await _context.Companies
                .Where(c => c.Deadline.HasValue && c.Deadline.Value <= now)
                .ToListAsync();

            foreach (var company in expiredCompanies)
            {
                // 查找該公司所有職缺
                var openings = await _context.Openings
                    .Where(o => o.CompanyId == company.CompanyId && o.ReleaseYN == true)
                    .ToListAsync();

                // 將職缺 ReleaseYN 設為 false
                foreach (var opening in openings)
                {
                    opening.ReleaseYN = false;
                }
                company.Deadline = null;

                // 保存變更
                await _context.SaveChangesAsync();
                Console.WriteLine($"公司: {company.CompanyName} 的所有職缺已經關閉.");
            }
        }
    }
}

