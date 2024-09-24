using JobHunting.Models;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Services
{
    public class PlanExpiredService
    {
        private readonly DuckContext _context;

        public PlanExpiredService(DuckContext context)
        {
            _context = context;
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
                // 發送提醒邏輯
                // 比如：使用EmailService發送郵件提醒公司
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

