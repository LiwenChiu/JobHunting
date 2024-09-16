using JobHunting.Areas.Candidates.ViewModels;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class C_ResumeController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public C_ResumeController(DuckCompaniesContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ResumeStorage()
        {
            return View();
        }
        public IActionResult ResumeStorageList()
        {
            return View();
        }




        //GET:Compaines/C_Resume/ResumeStorageJson

        [HttpGet]
        public async Task<IEnumerable<ResumeStorageViewModel>> ResumeStorageJson( int id)
            
        {

            var Result = await _context.Companies.Include(i => i.Resumes).ThenInclude(ti => ti.Tags)
                          .Include(i => i.Resumes).ThenInclude(ti => ti.TitleClasses)
                          .Include(i => i.Resumes).ThenInclude(ti => ti.Candidate)
                          .Include(i => i.Openings).ThenInclude(ti => ti.ResumeOpeningRecords)
                          .Where(rs => rs.CompanyId == id).ToListAsync();
             if(Result == null || !Result.Any())
            {
                return new List<ResumeStorageViewModel>();
            }
            var companies = Result.SelectMany(company => company.Resumes.Select(n => new ResumeStorageViewModel
            {
                ResumeId = n.ResumeId,
                CandidateId = n.Candidate.CandidateId,
                CompanyId = company.CompanyId,
                OpeningId = _context.Openings
                 .Where(o => o.ResumeOpeningRecords.Any(ror => ror.ResumeId == n.ResumeId))
                 .Select(o => o.OpeningId)
                 .FirstOrDefault(),
                Name = n.Candidate.Name,
                Address = n.Candidate.Address,
                Sex = n.Candidate.Sex,
                Birthday = n.Candidate.Birthday,
                Phone = n.Candidate.Phone,
                Degree = n.Candidate.Degree,
                Email = n.Candidate.Email,
                EmploymentStatus = n.Candidate.EmploymentStatus,
                Time = n.Time,
                Certification = n.Certification, /*!= null ? Convert.ToBase64String(n.Certification) : null,*/
                WorkExperience = n.WorkExperience,
                Autobiography = n.Autobiography,
                OpeningTitle = _context.ResumeOpeningRecords.Where(ror => ror.ResumeId == n.ResumeId).Select(ror => ror.OpeningTitle).FirstOrDefault(),
                Intro = n.Intro,
                TitleClassId = n.TitleClasses.Select(rtc => rtc.TitleClassId).ToList(),
                TagId = n.Tags.Select(t => t.TagId).ToList(),
                Headshot = n.Headshot, /*!= null ? Convert.ToBase64String(n.Headshot) : null,*/

            }));

            return companies;
        }



        [HttpPost]
        public async Task<IActionResult> SendInterview([FromBody] SendinterviewInputModel siv)
        {
            try
            {
                var company = await _context.Companies.FindAsync(siv.CompanyId);
                if (company == null)
                {
                    return NotFound(new { company = "Resume not found" });
                }

                Notification send = new Notification
                {
                     CompanyId = siv.CompanyId,
                     CandidateId = siv.CandidateId,
                     OpeningId = siv.OpeningId,
                     ResumeId = siv.ResumeId,
                     Status = siv.Status,
                     SubjectLine = siv.SubjectLine,
                     Content = siv.Content,
                     SendDate = siv.SendDate,
                     AppointmentDate = siv.AppointmentDate,
                     AppointmentTime = siv.AppointmentTime,
                     Address = siv.Address,
                     ReplyYN = siv.ReplyYN,
                };

                _context.Notifications.Add(send);
                await _context.SaveChangesAsync();
                
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "發送信件失敗", });
            }
            return Json(new { success = true, message = "發送信件成功", });
        }

        [HttpPost]
        public async Task<IActionResult> SendAdmission([FromBody] SendAdmissionInputModel siv)
        {
            try
            {
                var company = await _context.Companies.FindAsync(siv.CompanyId);
                if (company == null)
                {
                    return NotFound(new { company = "Resume not found" });
                }

                Notification send = new Notification
                {
                    CompanyId = siv.CompanyId,
                    CandidateId = siv.CandidateId,
                    OpeningId = siv.OpeningId,
                    ResumeId = siv.ResumeId,
                    Status = siv.Status,
                    SubjectLine = siv.SubjectLine,
                    Content = siv.Content,
                    SendDate = siv.SendDate,
                    AppointmentDate = siv.AppointmentDate,
                    AppointmentTime = siv.AppointmentTime,
                    Address = siv.Address,
                    ReplyYN = siv.ReplyYN,
                };

                _context.Notifications.Add(send);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "發送信件失敗", });
            }
            return Json(new { success = true, message = "發送信件成功", });
        }





        [HttpPost]
        public async Task<IActionResult> AddCompanyResumeLikeRecords([FromBody]AddCompanyResumeLikeRecordsViewModel arlv)
        {
            try
            {
                var query = "INSERT INTO CompanyResumeLikeRecords(CompanyId,ResumeId) VALUES (@CompanyId ,@ResumeId)";

                var companyIdParam = new SqlParameter("@CompanyId", arlv.CompanyId);
                var resumeIdParam = new SqlParameter("@ResumeId", arlv.ResumeId);

                await _context.Database.ExecuteSqlRawAsync(query, companyIdParam, resumeIdParam);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "履歷收藏失敗" });
            }
            

            return Json(new { success = true, message = "履歷已成功收藏" });
        }


        [HttpPost]
        public async Task<IActionResult> RemoveCompanyResumeLikeRecords([FromBody]RemoveCompanyResumeLikeRecordsViewModel rcrlv)
        {
            
            var query = "DELETE FROM CompanyResumeLikeRecords WHERE CompanyId = @CompanyId AND ResumeId = @ResumeId";

            var companyIdParam = new SqlParameter("@CompanyId", rcrlv.CompanyId);
            var resumeIdParam = new SqlParameter("@ResumeId", rcrlv.ResumeId);

            int rowsAffected = await _context.Database.ExecuteSqlRawAsync(query, companyIdParam, resumeIdParam);

            if (rowsAffected > 0)
            {
                return Json(new { success = true, message = "已删除暫存成功" });
            }
            else
            {
                return Json(new { success = false, message = "删除暫存失败" });
            }
        }






        [HttpGet]
        public JsonResult TitleCategoryJson()
        {
            return Json(_context.TitleCategories.Select(rtc => new
            {
                TitleCategoryId = rtc.TitleCategoryId,
                TitleCategoryName = rtc.TitleCategoryName
            }));
        }

        [HttpGet]
        public JsonResult TitleClassJson()
        {
            return Json(_context.TitleClasses.Select(rtc => new
            {
                TitleClassId = rtc.TitleClassId,
                TitleClassName = rtc.TitleClassName,
                TitleCategoryId = rtc.TitleCategoryId
            }));
        }

        [HttpGet]
        public JsonResult TagJson()
        {
            return Json(_context.Tags.Select(rt => new
            {
                TagId = rt.TagId,
                TagName = rt.TagName,
                TagClassId = rt.TagClassId
            }));
        }
        //GET:Companies/C_Resume/TagClassJson
        [HttpGet]
        public JsonResult TagClassJson()
        {
            return Json(_context.TagClasses.Select(rtc => new
            {
                TagClassId = rtc.TagClassId,
                TagClassName = rtc.TagClassName
            }));
        }


        //[HttpPost]
        //public async Task<IActionResult> 







        //GET:Compaines/C_Resume/ReceiveResume
        [HttpGet]
        public IActionResult ReceiveResume()
        {
            return View();
        }



        //GET:compaines/C_Resume/ReceiveResumeJson
        //[HttpGet]
        //public JsonResult ReceiveResumeJson()
        //{
        //    var Openings = _context.Openings;
        //    var ResumeOpeningRecords = _context.ResumeOpeningRecords;
        //    return Json(Openings.Select(p => new
        //    {
        //        Title = p.Title,
        //        TitleId = p.OpeningId,
        //        TitleClassName = p.TitleClasses.Select(tc => tc.TitleClassName).FirstOrDefault(),
        //        TitleClassId = p.TitleClasses.Select(tc => tc.TitleClassId).FirstOrDefault(),
        //        ApplyDate = p.ResumeOpeningRecords.Select(ror => ror.ApplyDate).FirstOrDefault(),
        //    }));
        //}






        //[HttpPost]
        //public async Task<IEnumerable<ReceiveResumeViewModel>> ReceiveResumeJson(int id)
        //{
        //    var Result =  await _context.Companies.Include(i => i.Resumes).ThenInclude(ti => ti.Tags)
        //                  .Include(i => i.Resumes).ThenInclude(ti => ti.TitleClasses)
        //                  .Include(i => i.Resumes).ThenInclude(ti => ti.Candidate)
        //                  .Include(i => i.Openings).ThenInclude(ti => ti.ResumeOpeningRecords)
        //                  .Where(rs => rs.CompanyId == id).ToListAsync();
        //    if (Result == null || !Result.Any())
        //    {
        //        return new List<ReceiveResumeViewModel>();
        //    }
        //    var companies = Result.SelectMany(company => company.Resumes.Select(n => new ReceiveResumeViewModel
        //    {
        //        ResumeId = n.ResumeId,
        //        CandidateId = n.Candidate.CandidateId,
        //        CompanyId = company.CompanyId,
        //        OpeningId = _context.Openings
        //         .Where(o => o.ResumeOpeningRecords.Any(ror => ror.ResumeId == n.ResumeId))
        //         .Select(o => o.OpeningId)
        //         .FirstOrDefault(),
        //        Name = n.Candidate.Name,
        //        Address = n.Candidate.Address,
        //        Sex = n.Candidate.Sex,
        //        Birthday = n.Candidate.Birthday,
        //        Phone = n.Candidate.Phone,
        //        Degree = n.Candidate.Degree,
        //        Email = n.Candidate.Email,
        //        EmploymentStatus = n.Candidate.EmploymentStatus,
        //        Time = n.Time,
        //        Certification = n.Certification, /*!= null ? Convert.ToBase64String(n.Certification) : null,*/
        //        WorkExperience = n.WorkExperience,
        //        Autobiography = n.Autobiography,
        //        OpeningTitle = _context.ResumeOpeningRecords.Where(ror => ror.ResumeId == n.ResumeId).Select(ror => ror.OpeningTitle).FirstOrDefault(),
        //        Intro = n.Intro,
        //        TitleClassId = n.TitleClasses.Select(rtc => rtc.TitleClassId).ToList(),
        //        TagId = n.Tags.Select(t => t.TagId).ToList(),
        //        Headshot = n.Headshot, /*!= null ? Convert.ToBase64String(n.Headshot) : null,*/
        //        ApplyDate = _context.ResumeOpeningRecords.Where(ror => ror.ResumeId == n.ResumeId).Select(ror => ror.ApplyDate).FirstOrDefault(),

        //    }));

        //    return companies;
        //}


        //投遞履歷
        [HttpPost]
        public async Task<IEnumerable<ReceiveResumeOutputModel>> ReceiveResumeres(int id)
        {
            var Result = await _context.ResumeOpeningRecords.Include(x => x.Opening).ThenInclude(x => x.Company)
                          .Include(x => x.Opening).ThenInclude(x => x.TitleClasses).Include(x => x.Opening).ThenInclude(x => x.Tags)
                          .Include(x => x.Resume).ThenInclude(x => x.Candidate).Include(x => x.Opening).ThenInclude(x => x.TitleClasses).ThenInclude(x => x.TitleCategory)
                          .Where(x => x.CompanyId == id)
                          .Select(n => new ReceiveResumeOutputModel
                          {
                              ResumeId = n.ResumeId,
                              CompanyId = n.CompanyId,
                              OpeningId = n.OpeningId,
                              Name = n.Resume.Candidate.Name,
                              Address = n.Resume.Candidate.Address,
                              Sex = n.Resume.Candidate.Sex,
                              Birthday = n.Resume.Candidate.Birthday,
                              Phone = n.Resume.Candidate.Phone,
                              Degree = n.Resume.Candidate.Degree,
                              Email = n.Resume.Candidate.Email,
                              EmploymentStatus = n.Resume.Candidate.EmploymentStatus,
                              Time = n.Resume.Time,
                              Intro = n.Resume.Intro,
                              Certification = n.Resume.Certification, /*!= null ? Convert.ToBase64String(n.Certification) : null,*/
                              WorkExperience = n.Resume.WorkExperience,
                              Autobiography = n.Resume.Autobiography,
                              OpeningTitle = n.OpeningTitle,
                              TitleClassId = n.Resume.TitleClasses.Select(rtc => rtc.TitleClassId).ToList(),
                              TagId = n.Resume.Tags.Select(r => r.TagId).ToList(), // 确保这里正确
                              Headshot = n.Resume.Headshot, /*!= null ? Convert.ToBase64String(n.Headshot) : null,*/
                              ApplyDate = n.ApplyDate,
                              InterviewYN = n.InterviewYN,
                              HireYN = n.HireYN,

                          }).ToListAsync();


            if (Result == null)
            {
                return new List<ReceiveResumeOutputModel>();
            }

            return Result;

        }


        //    [HttpPost]
        //    public async Task<IEnumerable<ReceiveResumeOutputModel>> ReceiveResumefilter([FromBody]ReceiveResumeinputModel rrim)
        //    {
        //        var Result = await _context.ResumeOpeningRecords.Include(x => x.Opening).ThenInclude(x => x.Company)
        //                      .Include(x => x.Opening).ThenInclude(x => x.TitleClasses).Include(x => x.Opening).ThenInclude(x => x.Tags)
        //                      .Include(x => x.Resume).ThenInclude(x => x.Candidate).Include(x => x.Opening).ThenInclude(x => x.TitleClasses).ThenInclude(x => x.TitleCategory).ToList()
        //                      .Where(x => x.CompanyId == rrim.CompanyId ||
        //                       x.OpeningTitle.Contains(rrim.OpeningTitle)||
        //                       x.Resume.Candidate.Name.Contains(rrim.Name)||
        //                       x.ApplyDate.HasValue && rrim.ApplyDate.HasValue &&
        //                       x.ApplyDate.Value.Year == rrim.ApplyDate.Value.Year &&
        //                       x.ApplyDate.Value.Month == rrim.ApplyDate.Value.Month)
        //                      .Select(n => new ReceiveResumeOutputModel
        //                      {
        //                          ResumeId = n.ResumeId,
        //                          CompanyId = n.CompanyId,
        //                          OpeningId = n.OpeningId,
        //                          Name = n.Resume.Candidate.Name,
        //                          Address = n.Resume.Candidate.Address,
        //                          Sex = n.Resume.Candidate.Sex,
        //                          Birthday = n.Resume.Candidate.Birthday,
        //                          Phone = n.Resume.Candidate.Phone,
        //                          Degree = n.Resume.Candidate.Degree,
        //                          Email = n.Resume.Candidate.Email,
        //                          EmploymentStatus = n.Resume.Candidate.EmploymentStatus,
        //                          Time = n.Resume.Time,
        //                          Intro = n.Resume.Intro,
        //                          Certification = n.Resume.Certification, /*!= null ? Convert.ToBase64String(n.Certification) : null,*/
        //                          WorkExperience = n.Resume.WorkExperience,
        //                          Autobiography = n.Resume.Autobiography,
        //                          OpeningTitle = n.OpeningTitle,
        //                          TitleClassId = n.Resume.TitleClasses.Select(rtc => rtc.TitleClassId).ToList(),
        //                          TagId = n.Resume.Tags.Select(r => r.TagId).ToList(), // 确保这里正确
        //                          Headshot = n.Resume.Headshot, /*!= null ? Convert.ToBase64String(n.Headshot) : null,*/
        //                          ApplyDate = n.ApplyDate,
        //                          InterviewYN = n.InterviewYN,
        //                          HireYN = n.HireYN,

        //                      }).ToListAsync();


        //        if (Result == null)
        //        {
        //            return new List<ReceiveResumeOutputModel>();
        //        }

        //        return Result;

        //    }





    }

}
