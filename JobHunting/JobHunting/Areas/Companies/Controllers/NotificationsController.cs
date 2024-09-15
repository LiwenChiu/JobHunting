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
            var companyNotifications = _context.Notifications.Include(n => n.Candidate).Include(n => n.Company).AsNoTracking()
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


        //POST: Companies/Notifications/GetNotification
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<NotificationCompanyModalOutputViewModel> GetNotification([FromBody][Bind("CompanyId,NotificationId")] NotificationCompanyModalInputViewModel ncmvm)
        {
            var notification = _context.Notifications.Include(n => n.Candidate).ThenInclude(c => c.Resumes).Include(n => n.Company).ThenInclude(c => c.Openings).AsNoTracking()
                .Where(n => n.CompanyId == ncmvm.CompanyId)
                .Where(n => n.NotificationId == ncmvm.NotificationId)
                .Select(n => new NotificationCompanyModalOutputViewModel
                {
                    NotificationId = n.NotificationId,
                    Status = n.Status,
                    SubjectLine = n.SubjectLine,
                    Content = n.Content,
                    AppointmentDate = n.AppointmentDate,
                    AppointmentTime = n.AppointmentTime,
                    Address = n.Address,
                    CompanyName = n.Company.CompanyName,
                    CandidateName = n.Candidate.Name,
                    OpeningTitle = n.Company.Openings.Where(o => o.OpeningId == n.OpeningId).Select(o => o.Title).Single(),
                    ResumeTitle = n.Candidate.Resumes.Where(r => r.ResumeId == n.ResumeId).Select(r => r.Title).Single(),
                    ReplyYN = n.ReplyYN,
                    Reply = n.Reply,
                }).Single();

            if (notification == null)
            {
                return new NotificationCompanyModalOutputViewModel();
            }

            return notification;
        }
    }
}
