using JobHunting.Areas.Admins.ViewModels;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class NotificationsController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public NotificationsController(DuckCompaniesContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //POST: Companies/Notifications/GetNotificationLess
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<NotificationCompanyFullOutputViewModel> GetNotificationLess([FromBody][Bind("CompanyId,filterInput")] NotificationsFilterViewModel nfvm)
        {
            var companyNotifications = _context.Notifications.Include(cn => cn.Candidate).Include(cn => cn.Company).AsNoTracking()
                .Select(cn => new
                {
                    NotificationId = cn.NotificationId,
                    CompanyId = cn.CompanyId,
                    CandidateId = cn.CandidateId,
                    OpeningId = cn.OpeningId,
                    ResumeId = cn.ResumeId,
                    Status = cn.Status,
                    SubjectLine = cn.SubjectLine,
                    Content = cn.Content,
                    SendDate = cn.SendDate,
                    AppointmentDate = cn.AppointmentDate,
                    CompanyName = cn.Company.CompanyName,
                    CandidateName = cn.Candidate.Name,
                })
                .Where(cn => cn.CompanyId == nfvm.CompanyId)
                .Where(cn => cn.CandidateName.Contains(nfvm.filterInput) ||
                             cn.Status.Contains(nfvm.filterInput) ||
                             cn.SubjectLine.Contains(nfvm.filterInput) ||
                             cn.Content.Contains(nfvm.filterInput))
                .OrderByDescending(cn => cn.SendDate)
                .Select(cn => new NotificationCompanyLessOutputViewModel
                {
                    NotificationId = cn.NotificationId,
                    OpeningId = cn.OpeningId,
                    ResumeId = cn.ResumeId,
                    Status = cn.Status,
                    SubjectLine = cn.SubjectLine,
                    AppointmentDate = cn.AppointmentDate,
                    CandidateName = cn.CandidateName,
                });

            var filterPaging = new NotificationCompanyFullOutputViewModel
            {
                totalDataCount = companyNotifications.Count(),
                companyNotificationsFilter = companyNotifications.Skip((nfvm.pageStart - 1) * 8).Take(8),
            };

            return filterPaging;
        }

        public async Task<> GetNotification([FromBody] int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return
            }

        }
    }
}
