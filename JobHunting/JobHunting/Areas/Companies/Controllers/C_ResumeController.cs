using JobHunting.Areas.Candidates.ViewModels;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using JobHunting.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JobHunting.Areas.Companies.Controllers
{
    [Authorize(Roles = "company")]
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
        public async Task<IEnumerable<ResumeStorageViewModel>> ResumeStorageJson()
        {
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return new List<ResumeStorageViewModel>(); // 或處理未授權訪問的情況
            }
            var Result = await _context.Companies.Include(i => i.Resumes).ThenInclude(ti => ti.Tags)
                          .Include(i => i.Resumes).ThenInclude(ti => ti.ResumeCertifications)
                          .Include(i => i.Resumes).ThenInclude(ti => ti.TitleClasses)
                          .Include(i => i.Resumes).ThenInclude(ti => ti.Candidate)
                          .Include(i => i.Openings).ThenInclude(ti => ti.ResumeOpeningRecords)
                          .Where(rs => rs.CompanyId.ToString() == CompanyId).ToListAsync();
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
                /*Certification = n.Certification,*/ /*!= null ? Convert.ToBase64String(n.Certification) : null,*/
                WorkExperience = n.WorkExperience,
                Autobiography = n.Autobiography,
                OpeningTitle = _context.ResumeOpeningRecords.Where(ror => ror.ResumeId == n.ResumeId).Select(ror => ror.OpeningTitle).FirstOrDefault(),
                Intro = n.Intro,
                TitleClassId = n.TitleClasses.Select(rtc => rtc.TitleClassId).ToList(),
                TagId = n.Tags.Select(t => t.TagId).ToList(),
                FileName = n.ResumeCertifications.Select(z => new { z.ResumeId, z.CertificationId, z.CertificationName }),
                Headshot = n.Headshot, /*!= null ? Convert.ToBase64String(n.Headshot) : null,*/
                ResumeOpeningRecordId = n.ResumeOpeningRecords.Select(r=>r.ResumeOpeningRecordId).FirstOrDefault(),
            }));

            return companies;
        }
        [HttpGet]
        public async Task<IEnumerable<GetOpenIngsOutputmodel>> GetOpenings()
        {
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return new List<GetOpenIngsOutputmodel>(); // 或處理未授權訪問的情況
            }

            var query = await _context.Openings.Include(a => a.Company).Where(rs => rs.CompanyId.ToString() == CompanyId).ToListAsync();
            
            var opening = query.Select(p => new GetOpenIngsOutputmodel
            {
                OpeningId = p.OpeningId,
                CompanyId = p.CompanyId,
                Title = p.Title
            });
            return opening;
        }


        [HttpPost]
        public async Task<IActionResult> SendInterview([FromBody] SendinterviewInputModel siv)
        {
            try
            {
                //var resumeOpeningRecordId = await _context.ResumeOpeningRecords.Where(c => c.ResumeOpeningRecordId == siv.ResumeOpeningRecordId).FirstOrDefaultAsync();

                var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(CompanyId) || !int.TryParse(CompanyId, out int companyId))
                {
                    return Unauthorized(new { message = "未授權訪問" });
                }

                var company = await _context.Companies.FindAsync(companyId);
                if (company == null)
                {
                    return NotFound(new { company = "Resume not found" });
                }


                if (siv.ResumeOpeningRecordId == null)
                {
                    ResumeOpeningRecord ror = new ResumeOpeningRecord
                    {
                        CompanyId = companyId,
                        OpeningId = siv.OpeningId,
                        ResumeId = siv.ResumeId,
                        ApplyDate = null,
                        InterviewYN = false,
                        HireYN = false,
                        OpeningTitle = siv.OpeningTitle,
                        CompanyName = company.CompanyName,
                    };
                    _context.ResumeOpeningRecords.Add(ror);
                    await _context.SaveChangesAsync();

                    Notification send = new Notification
                    {
                        CompanyId = companyId,
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
                        ReplyFirstYN = siv.ReplyFirstYN,
                        ResumeOpeningRecordId = ror.ResumeOpeningRecordId,
                    };

                    _context.Notifications.Add(send);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "發送信件成功", });
                }
                else {
                    Notification send = new Notification
                    {
                        CompanyId = companyId,
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
                        ReplyFirstYN = siv.ReplyFirstYN,
                        ResumeOpeningRecordId = siv.ResumeOpeningRecordId,
                    };

                    _context.Notifications.Add(send);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "發送信件成功", });
                }
                
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "發送信件失敗", });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> SendAdmission([FromBody] SendAdmissionInputModel siv)
        {
            try
            {

                var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(CompanyId) || !int.TryParse(CompanyId, out int companyId))
                {
                    return Unauthorized(new { message = "未授權訪問" });
                }

                var company = await _context.Companies.FindAsync(companyId);
                if (company == null)
                {
                    return NotFound(new { company = "Resume not found" });
                }
                //var resumeOpeningRecord = _context.ResumeOpeningRecords.Where(r => r.ResumeId == siv.ResumeId && r.OpeningId == siv.OpeningId).FirstOrDefault();
                //var ResumeOpeningRecordId = resumeOpeningRecord.ResumeOpeningRecordId;

                Notification send = new Notification
                {
                    CompanyId = companyId,
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
                    ReplyFirstYN = siv.ReplyFirstYN,
                    ResumeOpeningRecordId = siv.ResumeOpeningRecordId,
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
                var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(CompanyId))
                {
                    return Unauthorized(new { message = "未授權訪問" });
                }

                var query = "INSERT INTO CompanyResumeLikeRecords(CompanyId,ResumeId) VALUES (@CompanyId ,@ResumeId)";

                var companyIdParam = new SqlParameter("@CompanyId", CompanyId);
                var resumeIdParam = new SqlParameter("@ResumeId", arlv.ResumeId);

                await _context.Database.ExecuteSqlRawAsync(query, companyIdParam, resumeIdParam);
            }
            catch (Exception ex)
            {
                if(arlv.ResumeId != null)
                {
                    return Json(new { success = false, message = "收藏失敗。履歷已收藏過" });
                }
                else
                {
                    return Json(new { success = false, message = "履歷收藏失敗" });
                }
            }
            

            return Json(new { success = true, message = "履歷已成功收藏" });
        }


        [HttpPost]
        public async Task<IActionResult> RemoveCompanyResumeLikeRecords([FromBody]RemoveCompanyResumeLikeRecordsViewModel rcrlv)
        {
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return Unauthorized(new { message = "未授權訪問" });
            }

            var query = "DELETE FROM CompanyResumeLikeRecords WHERE CompanyId = @CompanyId AND ResumeId = @ResumeId";

            var companyIdParam = new SqlParameter("@CompanyId", CompanyId);
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
        public async Task<FileResult> DownloadFile(int resumeId, int certificationId)
        {
            ResumeCertification c = _context.ResumeCertifications.FirstOrDefault(f => f.ResumeId == resumeId && f.CertificationId == certificationId);
            byte[] FileContent = c.FileData;
            var fileName = HttpUtility.UrlPathEncode(c.CertificationName);  //檔名去除無效字符
            ContentDisposition cd = new ContentDisposition
            {
                FileName = fileName,// 設定下載檔案名稱
                // Inline= false,        // 禁止直接顯示檔案內容
            };
            Response.Headers.Append(HeaderNames.ContentDisposition, cd.ToString());
            var fs = FileContent;
            return File(fs, MediaTypeNames.Application.Octet);
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

        

        //POST: compaines/C_Resume/ReceiveResumefilter
        [HttpPost]
        public async Task<IEnumerable<ReceiveResumeOutputModel>> ReceiveResumefilter([FromBody][Bind("ApplyDate", "InterviewYN", "HireYN", "Name", "OpeningTitle")] ReceiveResumeinputModel rrim)
        {
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return new List<ReceiveResumeOutputModel>(); // 或處理未授權訪問的情況
            }

            var query = _context.ResumeOpeningRecords.Include(x => x.Opening).ThenInclude(x => x.Company)
                          .Include(x => x.Opening).ThenInclude(x => x.TitleClasses).Include(x => x.Opening).ThenInclude(x => x.Tags)
                          .Include(x => x.Resume).ThenInclude(x => x.Candidate).Include(x => x.Resume).ThenInclude(x => x.ResumeCertifications).Include(x => x.Opening).ThenInclude(x => x.TitleClasses).ThenInclude(x => x.TitleCategory).AsNoTracking()
                          .Where(x => x.CompanyId.ToString() == CompanyId &&
                                 (x.OpeningTitle.Contains(rrim.OpeningTitle) ||
                                 x.Resume.Candidate.Name.Contains(rrim.Name) ||
                                 x.ApplyDate.ToString().Contains(rrim.ApplyDate)));
                             //x.ApplyDate.HasValue && rrim.ApplyDate.HasValue &&
                             //x.ApplyDate.Value.Year == rrim.ApplyDate.Value.Year &&
                             //x.ApplyDate.Value.Month == rrim.ApplyDate.Value.Month&&
                             //x.ApplyDate.Value.Day == rrim.ApplyDate.Value.Day)

                            //篩選 InterviewYN 的value
                             if (rrim.InterviewYN.HasValue)
                             {
                                 query = query.Where(x => x.InterviewYN == rrim.InterviewYN.Value);
                             }
                             // 篩選是否錄取
                             if (rrim.HireYN.HasValue)
                             {
                                 query = query.Where(x => x.HireYN == rrim.HireYN.Value);
                             }

                            var result = await query
                          .Select(n => new ReceiveResumeOutputModel
                          {
                              ResumeId = n.ResumeId,
                              CompanyId = int.Parse(CompanyId),
                              OpeningId = n.OpeningId,
                              CandidateId = n.Resume.Candidate.CandidateId,
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
                              FileName = n.Resume.ResumeCertifications.Select(z => new { z.ResumeId, z.CertificationId, z.CertificationName }),
                              WorkExperience = n.Resume.WorkExperience,
                              Autobiography = n.Resume.Autobiography,
                              OpeningTitle = n.OpeningTitle,
                              TitleClassId = n.Resume.TitleClasses.Select(rtc => rtc.TitleClassId).ToList(),
                              TagId = n.Resume.Tags.Select(r => r.TagId).ToList(), 
                              Headshot = n.Resume.Headshot, /*!= null ? Convert.ToBase64String(n.Headshot) : null,*/
                              ApplyDate = n.ApplyDate,
                              InterviewYN = n.InterviewYN,
                              HireYN = n.HireYN,
                              ResumeOpeningRecordId = n.ResumeOpeningRecordId,
                          }).ToListAsync();


            if (result == null)
            {
                return new List<ReceiveResumeOutputModel>();
            }

            return result;

        }





    }

}
