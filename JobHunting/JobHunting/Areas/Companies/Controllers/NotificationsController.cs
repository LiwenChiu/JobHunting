using JobHunting.Areas.Admins.ViewModels;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        public async Task<NotificationCompanyFullOutputViewModel> GetNotificationLess([FromBody][Bind("filterInput,pageStart")] NotificationsFilterViewModel nfvm)
        {
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return new NotificationCompanyFullOutputViewModel(); // 或處理未授權訪問的情況
            }
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
                    ReplyYN = cn.ReplyYN,
                    CandidateName = cn.Candidate.Name,
                })
                .Where(cn => cn.CompanyId.ToString() == CompanyId)
                .Where(cn => cn.CandidateName.Contains(nfvm.filterInput) ||
                             cn.Status.Contains(nfvm.filterInput) ||
                             cn.SubjectLine.Contains(nfvm.filterInput) ||
                             cn.Content.Contains(nfvm.filterInput))
                .OrderByDescending(cn => cn.ReplyYN)
                .OrderByDescending(cn => cn.SendDate)
                .Select(cn => new NotificationCompanyLessOutputViewModel
                {
                    NotificationId = cn.NotificationId,
                    OpeningId = cn.OpeningId,
                    ResumeId = cn.ResumeId,
                    Status = cn.Status,
                    SubjectLine = cn.SubjectLine,
                    AppointmentDate = cn.AppointmentDate,
                    ReplyYN = cn.ReplyYN,
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
        public async Task<NotificationCompanyModalOutputViewModel> GetNotification([FromBody][Bind("NotificationId")] NotificationCompanyModalInputViewModel ncmvm)
        {
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return new NotificationCompanyModalOutputViewModel(); // 或處理未授權訪問的情況
            }
            var notification = _context.Notifications.Include(n => n.Candidate).ThenInclude(c => c.Resumes).Include(n => n.Company).ThenInclude(c => c.Openings).AsNoTracking()
                .Where(n => n.CompanyId.ToString() == CompanyId)
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
                    ReplyFirstYN = n.ReplyFirstYN,
                    ReplyYN = n.ReplyYN,
                    Reply = n.Reply,
                    ReplyTime = n.ReplyTime,
                }).Single();

            if (notification == null)
            {
                return new NotificationCompanyModalOutputViewModel();
            }

            return notification;
        }

        //POST: Companies/Notifications/ConfirmReply
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<NotificationCompanyConfirmReplyOutputViewModel> ConfirmReply([FromBody][Bind("NotificationId")] NotificationCompanyConfirmReplyInputViewModel nccrivm)
        {
            if (!ModelState.IsValid)
            {
                return new NotificationCompanyConfirmReplyOutputViewModel
                {
                    AlertText = "錯誤",
                    AlertStatus = false,
                };
            }
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return new NotificationCompanyConfirmReplyOutputViewModel(); // 或處理未授權訪問的情況
            }
            var notification = await _context.Notifications.Where(n => n.CompanyId.ToString() == CompanyId).Where(n => n.NotificationId == nccrivm.NotificationId).SingleAsync();
            if (notification == null)
            {
                return new NotificationCompanyConfirmReplyOutputViewModel {
                    AlertText = "錯誤",
                    AlertStatus = false,
                };
            }

            notification.ReplyYN = false;

            _context.Entry(notification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                return new NotificationCompanyConfirmReplyOutputViewModel
                {
                    AlertText = "錯誤",
                    AlertStatus = false,
                };
            }

            return new NotificationCompanyConfirmReplyOutputViewModel
            {
                AlertText = "成功",
                AlertStatus = true,
            };
        }

        //POST: Companies/Notifications/DeleteNotification
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<NotificationCompanyDeleteNotificationOutputViewModel> DeleteNotification([FromBody][Bind("NotificationId")] NotificationCompanyDeleteNotificationInputViewModel ncdnivm)
        {
            if (!ModelState.IsValid)
            {
                return new NotificationCompanyDeleteNotificationOutputViewModel
                {
                    AlertText = "錯誤",
                    AlertStatus = false,
                };
            }
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return new NotificationCompanyDeleteNotificationOutputViewModel(); // 或處理未授權訪問的情況
            }
            var notification = await _context.Notifications.Where(n => n.CompanyId.ToString() == CompanyId).Where(n => n.NotificationId == ncdnivm.NotificationId).SingleAsync();
            if (notification == null)
            {
                return new NotificationCompanyDeleteNotificationOutputViewModel
                {
                    AlertText = "錯誤",
                    AlertStatus = false,
                };
            }

            notification.CompanyId = null;

            _context.Entry(notification).State = EntityState.Modified;

            if (notification.CompanyId == null && notification.CandidateId == null)
            {
                _context.Notifications.Remove(notification);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new NotificationCompanyDeleteNotificationOutputViewModel
                {
                    AlertText = "錯誤",
                    AlertStatus = false,
                };
            }

            return new NotificationCompanyDeleteNotificationOutputViewModel
            {
                AlertText = "成功",
                AlertStatus = true,
            };
        }
    }
}
