using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Area("Candidates")]
    public class NotificationsController : Controller
    {
        private readonly DuckCandidatesContext _context;

        public NotificationsController(DuckCandidatesContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //POST: Candidates/Notifications/GetNotificationLess
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<NotificationCandidateFullOutputViewModel> GetNotificationLess([FromBody][Bind("CandidateId,filterInput,pageStart")] NotificationsFilterViewModel nfvm)
        {
            var candidateNotifications = _context.Notifications.Include(n => n.Candidate).Include(n => n.Company).AsNoTracking()
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
                })
                .Where(cn => cn.CandidateId == nfvm.CandidateId)
                .Where(cn => cn.CompanyName.Contains(nfvm.filterInput) ||
                             cn.Status.Contains(nfvm.filterInput) ||
                             cn.SubjectLine.Contains(nfvm.filterInput) ||
                             cn.Content.Contains(nfvm.filterInput))
                .OrderByDescending(cn => cn.SendDate)
                .Select(cn => new NotificationCandidateLessOutputViewModel
                {
                    NotificationId = cn.NotificationId,
                    CompanyId = cn.CompanyId,
                    OpeningId = cn.OpeningId,
                    ResumeId = cn.ResumeId,
                    Status = cn.Status,
                    SubjectLine = cn.SubjectLine,
                    AppointmentDate = cn.AppointmentDate,
                    CompanyName = cn.CompanyName,
                });

            var filterPaging = new NotificationCandidateFullOutputViewModel
            {
                totalDataCount = candidateNotifications.Count(),
                candidateNotificationsFilter = candidateNotifications.Skip((nfvm.pageStart - 1) * 8).Take(8),
            };

            return filterPaging;
        }


        //POST: Candidates/Notifications/GetNotification
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<NotificationCandidateModalOutputViewModel> GetNotification([FromBody][Bind("CandidateId,NotificationId")] NotificationCandidateModalInputViewModel ncmvm)
        {
            var notification = await _context.Notifications.Include(n => n.Candidate).ThenInclude(c => c.Resumes).Include(n => n.Company).ThenInclude(c => c.Openings).AsNoTracking()
                .Where(n => n.CandidateId == ncmvm.CandidateId)
                .Where(n => n.NotificationId == ncmvm.NotificationId)
                .Select(n => new NotificationCandidateModalOutputViewModel
                {
                    NotificationId = n.NotificationId,
                    CompanyId = n.CompanyId,
                    Status = n.Status,
                    SubjectLine = n.SubjectLine,
                    Content = n.Content,
                    AppointmentDate = n.AppointmentDate,
                    AppointmentTime = n.AppointmentTime,
                    Address = n.Address,
                    CompanyName = n.Company.CompanyName,
                    CandidateName = n.Candidate.Name,
                    OpeningTitle = n.Company.Openings.Where(o => o.OpeningId == n.OpeningId).Select(o => o.Title).FirstOrDefault(),
                    ResumeTitle = n.Candidate.Resumes.Where(r => r.ResumeId == n.ResumeId).Select(r => r.Title).FirstOrDefault(),
                    ReplyFirstYN = n.ReplyFirstYN,
                    ReplyYN = n.ReplyYN,
                    Reply = n.Reply,
                    ReplyTime = n.ReplyTime,
                    EditReplyYN = false,
                }).FirstOrDefaultAsync();

            if (notification == null)
            {
                return new NotificationCandidateModalOutputViewModel();
            }

            return notification;
        }

        //POST: Candidates/Notifications/SendFirstReply
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<NotificationSendReplyOutputViewModel> SendFirstReply([FromBody][Bind("CandidateId,NotificationId,Reply")] NotificationSendReplyInputViewModel nsrivm)
        {
            if (!ModelState.IsValid)
            {
                return new NotificationSendReplyOutputViewModel {
                    AlertText = "格式不正確",
                    AlertStatus = false,
                };
            }

            var candidateNotification = await _context.Notifications.FindAsync(nsrivm.NotificationId);

            if (candidateNotification == null || candidateNotification.CandidateId != nsrivm.CandidateId)
            {
                return new NotificationSendReplyOutputViewModel
                {
                    AlertText = "找不到應徵信件",
                    AlertStatus = false,
                };
            }

            candidateNotification.Reply = nsrivm.Reply;
            candidateNotification.ReplyFirstYN = true;
            candidateNotification.ReplyYN = true;
            candidateNotification.ReplyTime = DateTime.Now;

            _context.Entry(candidateNotification).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                return new NotificationSendReplyOutputViewModel
                {
                    AlertText = "回覆失敗",
                    AlertStatus = false,
                };
            }

            return new NotificationSendReplyOutputViewModel
            {
                AlertText = "回覆成功",
                AlertStatus = true,
            };
        }

        //POST: Candidates/Notifications/SendReply
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<NotificationSendReplyOutputViewModel> SendReply([FromBody][Bind("CandidateId,NotificationId,Reply")] NotificationSendReplyInputViewModel nsrivm)
        {
            if (!ModelState.IsValid)
            {
                return new NotificationSendReplyOutputViewModel
                {
                    AlertText = "格式不正確",
                    AlertStatus = false,
                };
            }

            var candidateNotification = await _context.Notifications.FindAsync(nsrivm.NotificationId);

            if (candidateNotification == null || candidateNotification.CandidateId != nsrivm.CandidateId)
            {
                return new NotificationSendReplyOutputViewModel
                {
                    AlertText = "找不到應徵信件",
                    AlertStatus = false,
                };
            }

            candidateNotification.Reply = nsrivm.Reply;
            candidateNotification.ReplyYN = true;
            candidateNotification.ReplyTime = DateTime.Now;

            _context.Entry(candidateNotification).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new NotificationSendReplyOutputViewModel
                {
                    AlertText = "修改失敗",
                    AlertStatus = false,
                };
            }

            return new NotificationSendReplyOutputViewModel
            {
                AlertText = "修改成功",
                AlertStatus = true,
            };
        }

        //POST: Candidates/Notifications/DeleteNotification
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<NotificationCandidateDeleteNotificationOutputViewModel> DeleteNotification([FromBody][Bind("CandidateId,NotificationId")] NotificationCandidateDeleteNotificationInputViewModel ncdnivm)
        {
            if (!ModelState.IsValid)
            {
                return new NotificationCandidateDeleteNotificationOutputViewModel
                {
                    AlertText = "錯誤",
                    AlertStatus = false,
                };
            }

            var notification = await _context.Notifications.Where(n => n.CandidateId == ncdnivm.CandidateId).Where(n => n.NotificationId == ncdnivm.NotificationId).SingleAsync();
            if (notification == null)
            {
                return new NotificationCandidateDeleteNotificationOutputViewModel
                {
                    AlertText = "錯誤",
                    AlertStatus = false,
                };
            }

            if (notification.CompanyId == null)
            {
                _context.Notifications.Remove(notification);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new NotificationCandidateDeleteNotificationOutputViewModel
                {
                    AlertText = "錯誤",
                    AlertStatus = false,
                };
            }

            return new NotificationCandidateDeleteNotificationOutputViewModel
            {
                AlertText = "成功",
                AlertStatus = true,
            };
        }
    }
}
