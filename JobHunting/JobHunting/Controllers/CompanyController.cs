using JobHunting.Areas.Companies.ViewModel;
using JobHunting.Models;
using JobHunting.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.Mime;
using System.Security.Claims;
using System.Web;

namespace JobHunting.Controllers
{
    public class CompanyController : Controller
    {
        private readonly DuckContext _context; IWebHostEnvironment _hostingEnvironment;
        public CompanyController(DuckContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(AuthenticationSchemes = $"{CookieAuthenticationDefaults.AuthenticationScheme},AdminScheme", Roles = "company,admin")]
        public async Task<IActionResult> Index()
        {
                return View();
        }
        public async Task<IActionResult> ResumeDetail(int? resumeID)
        {
            if (resumeID == null)
            {
                return NotFound();
            }
            var today = DateOnly.FromDateTime(DateTime.Now);
            var companyIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (companyIdClaim == null)
            {
                return Json(_context.Resumes.Include(a => a.Candidate).Include(x => x.Tags).Include(y => y.TitleClasses).Include(z => z.Companies)
                .Where(y => y.ResumeId == resumeID)
                .Select(c => new ResumesIntroViewModel
                {
                    ResumeId = c.ResumeId,
                    CandidateId = c.CandidateId,
                    Title = c.Title,
                    Intro = c.Intro,
                    Autobiography = c.Autobiography,
                    WorkExperience = c.WorkExperience,
                    WishAddress = c.Address,
                    WishTime = c.Time,
                    Name = c.Candidate.Name,
                    Phone = c.Candidate.Phone,
                    Email = c.Candidate.Email,
                    EmploymentStatus = c.Candidate.EmploymentStatus,
                    MilitaryService = c.Candidate.MilitaryService,
                    Sex = c.Candidate.Sex,
                    Degree = c.Candidate.Degree,
                    Address = c.Candidate.Address,
                    TagObj = c.Tags.Select(z => new { z.TagId, z.TagName }),
                    TitleObj = c.TitleClasses.Select(z => new { z.TitleClassId, z.TitleClassName }),
                    FileName = c.ResumeCertifications.Select(z => new { z.CertificationId, z.CertificationName }),
                    Age = c.Candidate.Birthday.HasValue ? CalculateAge(c.Candidate.Birthday.Value, today) : 0,
                    LikeYN = null,
                }));
            }

            int companyId = int.Parse(companyIdClaim.Value);

            
            return Json(_context.Resumes.Include(a => a.Candidate).Include(x => x.Tags).Include(y => y.TitleClasses).Include(z => z.Companies).Include(q => q.ResumeCertifications)
                .Where(y => y.ResumeId == resumeID)
                .Select(c => new ResumesIntroViewModel
                {
                    ResumeId = c.ResumeId,
                    CandidateId = c.CandidateId,
                    Title = c.Title,
                    Intro = c.Intro,
                    Autobiography = c.Autobiography,
                    WorkExperience = c.WorkExperience,
                    WishAddress = c.Address,
                    WishTime = c.Time,
                    Name = c.Candidate.Name,
                    Phone = c.Candidate.Phone,
                    Email = c.Candidate.Email,
                    EmploymentStatus = c.Candidate.EmploymentStatus,
                    MilitaryService = c.Candidate.MilitaryService,
                    Sex = c.Candidate.Sex,
                    Degree = c.Candidate.Degree,
                    Address = c.Candidate.Address,
                    TagObj = c.Tags.Select(z => new { z.TagId, z.TagName }),
                    TitleObj = c.TitleClasses.Select(z => new { z.TitleClassId, z.TitleClassName}),
                    FileName = c.ResumeCertifications.Select(z => new { z.CertificationId, z.CertificationName }),
                    Age = c.Candidate.Birthday.HasValue ? CalculateAge(c.Candidate.Birthday.Value, today) : 0,
                    LikeYN = c.Companies.Where(x => x.CompanyId == companyId).FirstOrDefault() != null,
                }));
        }
        public async Task<IActionResult> ResumeIntro(int? candidateId, int? resumeId)
        {
            if (resumeId == null)
            {
                return NotFound();
            }
            else
            {
                var resume = await _context.Resumes.Include(a => a.Candidate).Where(c => c.ResumeId == resumeId && c.ReleaseYN == true).Select(c => new ResumeIDViewModel
                {
                    ResumeId = c.ResumeId,
                    CandidateId = c.CandidateId,
                    Title = c.Title,
                }).FirstOrDefaultAsync(m => m.CandidateId == candidateId);
                return View(resume);
            }
           
        }
        public async Task<CompanyResumeListViewModel> SelectIndexList([FromBody] ResumeInputModel resume)
        {
            EditResume(resume);
            var today = DateOnly.FromDateTime(DateTime.Now);
            var companyIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (companyIdClaim == null)
            {
                var sourceUnLogin = _context.Resumes.Include(a => a.Candidate).Include(x => x.Tags).Include(z => z.Companies).Where(b => b.ReleaseYN == true).ToList()
                    .Select(c => new
                    {
                        ResumeID = c.ResumeId,
                        CandidateID = c.CandidateId,
                        Intro = c.Intro,
                        Autobiography = c.Autobiography,
                        WorkExperience = c.WorkExperience,
                        WishAddress = c.Address,
                        Name = c.Candidate.Name,
                        Sex = c.Candidate.Sex,
                        Birthday = c.Candidate.Birthday,
                        Degree = c.Candidate.Degree,
                        Address = c.Candidate.Address,
                        skill = c.Tags.Select(z => new { z.TagId, z.TagName }),
                        Age = c.Candidate.Birthday.HasValue ? CalculateAge(c.Candidate.Birthday.Value, today) : 0,
                        LikeYN = false,
                    });
                if (!resume.AreaName.IsNullOrEmpty())
                {
                    sourceUnLogin = sourceUnLogin.Where(b =>
                                b.Address.Contains(resume.AreaName));
                }
                if (!resume.Edu.IsNullOrEmpty())
                {
                    sourceUnLogin = sourceUnLogin.Where(b =>
                                b.Degree == resume.Edu);
                }
                if (resume.Skill != null)
                {
                    sourceUnLogin = sourceUnLogin.Where(b =>
                        b.skill.Any(z => z.TagId == resume.Skill));
                }
                if (!resume.searchText.IsNullOrEmpty())
                {
                    sourceUnLogin = sourceUnLogin.Where(b =>
                            resume.searchText.Any(c => b.Name.IndexOf(c, StringComparison.OrdinalIgnoreCase) >= 0) ||  //不區分英文字母大小寫，逐一檢查
                            b.Sex == resume.Sex ||
                            b.Age.ToString().Contains(resume.searchText) ||
                            b.Address.Contains(resume.searchText) ||
                            b.Degree.Contains(resume.searchText));
                }
                if (resume.searchText == "" && resume.Skill == null && resume.Edu == "" && resume.AreaName == "")
                {
                    sourceUnLogin = sourceUnLogin;
                }
                var temp = sourceUnLogin.Select(x => new CompanyResumes
                {
                    ResumeID = x.ResumeID,
                    CandidateID = x.CandidateID,
                    Intro = x.Intro,
                    Autobiography = x.Autobiography,
                    Address = x.Address,
                    Name = x.Name,
                    Sex = x.Sex,
                    Age = x.Age,
                    WishAddress = x.WishAddress,
                    Degree = x.Degree,
                    TagObj = x.skill,
                    LikeYN = null,
                });
                var resumesSelectOutput = new CompanyResumeListViewModel();
                resumesSelectOutput.TotalDataCount = temp.Count();
                resumesSelectOutput.ResumeOutput = temp.Skip((resume.CurrentPage - 1) * resume.Perpage).Take(resume.Perpage);
                return resumesSelectOutput;
            }
            else
            {
                var companyId = int.Parse(companyIdClaim.Value);
                var sourceLogin = _context.Resumes.Include(a => a.Candidate).Include(x => x.Tags).Include(z => z.Companies).Where(b => b.ReleaseYN == true).ToList()
                    .Select(c => new
                    {
                        ResumeID = c.ResumeId,
                        CandidateID = c.CandidateId,
                        Title = c.Title,
                        Intro = c.Intro,
                        Autobiography = c.Autobiography,
                        WorkExperience = c.WorkExperience,
                        WishAddress = c.Address,
                        Name = c.Candidate.Name,
                        Sex = c.Candidate.Sex,
                        Birthday = c.Candidate.Birthday,
                        Degree = c.Candidate.Degree,
                        Address = c.Candidate.Address,
                        skill = c.Tags.Select(z => new { z.TagId, z.TagName }),
                        Age = c.Candidate.Birthday.HasValue ? CalculateAge(c.Candidate.Birthday.Value, today) : 0,
                        LikeYN = c.Companies.Where(a => a.CompanyId == companyId).FirstOrDefault() != null,
                    });
                if (!resume.AreaName.IsNullOrEmpty())
                {
                    sourceLogin = sourceLogin.Where(b =>
                                b.Address.Contains(resume.AreaName));
                }
                if (!resume.Edu.IsNullOrEmpty())
                {
                    sourceLogin = sourceLogin.Where(b =>
                                b.Degree == resume.Edu);
                }
                if (resume.Skill != null)
                {
                    sourceLogin = sourceLogin.Where(b =>
                        b.skill.Any(z => z.TagId == resume.Skill));
                }
                if (!resume.searchText.IsNullOrEmpty())
                {
                    sourceLogin = sourceLogin.Where(b =>
                            resume.searchText.Any(c => b.Name.IndexOf(c, StringComparison.OrdinalIgnoreCase) >= 0) ||  //不區分英文字母大小寫，逐一檢查
                            b.Sex == resume.Sex ||
                            b.Age.ToString().Contains(resume.searchText) ||
                            b.Address.Contains(resume.searchText) ||
                            b.Degree.Contains(resume.searchText));
                }
                if (resume.searchText == "" && resume.Skill == null && resume.Edu == "" && resume.AreaName == "")
                {
                    sourceLogin = sourceLogin;
                }
                var temp = sourceLogin.Select(x => new CompanyResumes
                {
                    ResumeID = x.ResumeID,
                    CandidateID = x.CandidateID,
                    Title = x.Title,
                    Intro = x.Intro,
                    Autobiography = x.Autobiography,
                    Address = x.Address,
                    Name = x.Name,
                    Sex = x.Sex,
                    Age = x.Age,
                    WorkExperience = x.WorkExperience,
                    WishAddress = x.WishAddress,
                    Degree = x.Degree,
                    TagObj = x.skill,
                    LikeYN = x.LikeYN,
                });
                var resumesSelectOutput = new CompanyResumeListViewModel();
                resumesSelectOutput.TotalDataCount = temp.Count();
                resumesSelectOutput.ResumeOutput = temp.Skip((resume.CurrentPage - 1) * resume.Perpage).Take(resume.Perpage);
                return resumesSelectOutput;
            }
        }

        public async Task<IActionResult> TagClasses()
        {
            return Json(_context.TagClasses.Include(a => a.Tags).Select(p => new TagSelectList
            {
                TagClassId = p.TagClassId,
                TagClassName = p.TagClassName,
                TagObj = p.Tags.Select(z => new { z.TagId, z.TagName })
            }));
        }
        public async Task<IActionResult> EduSelectInput()
        {
            return Json(_context.Candidates.Select(p => new EduSelectViewModel
            {
                Degree = p.Degree
            }));
        }
        public async Task<IActionResult> GetOpenings()
        {
            var companyIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (companyIdClaim == null)
            {
                return BadRequest("未找到公司 ID");
            }

            int companyId = int.Parse(companyIdClaim.Value);

            return Json(_context.Openings.Include(a => a.Company).Where(c => c.CompanyId == companyId).Select(p => new OpeningsListViewModel
            {
                OpeningId = p.OpeningId,
                CompanyId = p.CompanyId,
                Title = p.Title
            }));
        }
        [HttpPost]
        public async Task<string> AddInterview([FromBody]AddInterviewViewModel letter)
        {
            var companyIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (companyIdClaim == null)
            {
                return "未找到公司 ID"; // 或返回其他適當的錯誤
            }
            else
            {
                var CompanyId = int.Parse(companyIdClaim.Value);
                var today = DateOnly.FromDateTime(DateTime.Now);
                var company = await _context.Companies.FindAsync(CompanyId);
                var hasData = await _context.ResumeOpeningRecords.AnyAsync(c => c.ResumeOpeningRecordId == letter.ResumeOpeningRecordId);

                if(letter.ResumeOpeningRecordId  == null)
                {
                    ResumeOpeningRecord ror = new ResumeOpeningRecord
                    {
                        CompanyId = CompanyId,
                        OpeningId = letter.OpeningId,
                        ResumeId = Convert.ToInt32(letter.ResumeId),
                        ApplyDate = null,
                        InterviewYN = false,
                        HireYN = false,
                        OpeningTitle = letter.OpeningTitle,
                        CompanyName = company.CompanyName,
                    };
                    _context.ResumeOpeningRecords.Add(ror);
                    await _context.SaveChangesAsync();

                    Notification notificationLetter = new Notification();
                    if (letter != null)
                    {
                        notificationLetter.CompanyId = CompanyId;
                        notificationLetter.CandidateId = Convert.ToInt32(letter.CandidateId);
                        notificationLetter.OpeningId = letter.OpeningId;
                        notificationLetter.ResumeId = Convert.ToInt32(letter.ResumeId);
                        notificationLetter.Status = letter.Status;
                        notificationLetter.SubjectLine = letter.SubjectLine;
                        notificationLetter.Content = letter.Content;
                        notificationLetter.Address = letter.Address;
                        notificationLetter.SendDate = today;
                        notificationLetter.AppointmentDate = letter.AppointmentDate;
                        notificationLetter.AppointmentTime = letter.AppointmentTime;
                        notificationLetter.ResumeOpeningRecordId = ror.ResumeOpeningRecordId;
                        _context.Notifications.Add(notificationLetter);
                        await _context.SaveChangesAsync();
                        return "發送面試成功";
                    }
                    else
                    {
                        return "請輸入完整面試資訊";
                    }

                }else if (hasData)
                {
                    ResumeOpeningRecord ror = new ResumeOpeningRecord
                    {
                        CompanyId = CompanyId,
                        OpeningId = letter.OpeningId,
                        ResumeId = Convert.ToInt32(letter.ResumeId),
                        ApplyDate = null,
                        InterviewYN = false,
                        HireYN = false,
                        OpeningTitle = letter.OpeningTitle,
                        CompanyName = company.CompanyName,
                    };
                    _context.ResumeOpeningRecords.Add(ror);
                    await _context.SaveChangesAsync();

                    Notification notificationLetter = new Notification();
                    if (letter != null)
                    {
                        notificationLetter.CompanyId = CompanyId;
                        notificationLetter.CandidateId = Convert.ToInt32(letter.CandidateId);
                        notificationLetter.OpeningId = letter.OpeningId;
                        notificationLetter.ResumeId = Convert.ToInt32(letter.ResumeId);
                        notificationLetter.Status = letter.Status;
                        notificationLetter.SubjectLine = letter.SubjectLine;
                        notificationLetter.Content = letter.Content;
                        notificationLetter.Address = letter.Address;
                        notificationLetter.SendDate = today;
                        notificationLetter.AppointmentDate = letter.AppointmentDate;
                        notificationLetter.AppointmentTime = letter.AppointmentTime;
                        notificationLetter.ResumeOpeningRecordId = ror.ResumeOpeningRecordId;
                        _context.Notifications.Add(notificationLetter);
                        await _context.SaveChangesAsync();
                        return "發送面試成功";
                    }
                    else
                    {
                        return "請輸入完整面試資訊";
                    }

                }
                else
                {
                    Notification notificationLetter = new Notification();
                    if (letter != null)
                    {
                        notificationLetter.CompanyId = CompanyId;
                        notificationLetter.CandidateId = Convert.ToInt32(letter.CandidateId);
                        notificationLetter.OpeningId = letter.OpeningId;
                        notificationLetter.ResumeId = Convert.ToInt32(letter.ResumeId);
                        notificationLetter.Status = letter.Status;
                        notificationLetter.SubjectLine = letter.SubjectLine;
                        notificationLetter.Content = letter.Content;
                        notificationLetter.Address = letter.Address;
                        notificationLetter.SendDate = today;
                        notificationLetter.AppointmentDate = letter.AppointmentDate;
                        notificationLetter.AppointmentTime = letter.AppointmentTime;
                        notificationLetter.ResumeOpeningRecordId = letter.ResumeOpeningRecordId;
                        _context.Notifications.Add(notificationLetter);
                        await _context.SaveChangesAsync();
                        return "發送面試成功";
                    }
                    else
                    {
                        return "請輸入完整面試資訊";
                    }
                }

            }
            
        }
        [HttpPost]
        public async Task<string> AddFavorite([FromBody] AddFavoriteResumeViewModel favorite)
        {
            try
            {
                var companyIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (companyIdClaim == null)
                {
                    return "無法獲取使用者 ID，請重新登入";
                }

                var companyId = int.Parse(companyIdClaim.Value);
                var query = "INSERT INTO CompanyResumeLikeRecords(CompanyId,ResumeId) VALUES (@CompanyId ,@ResumeId)";
                var companyIdParam = new SqlParameter("@CompanyId", Convert.ToInt32(companyId));
                var resumeIdParam = new SqlParameter("@ResumeId", Convert.ToInt32(favorite.ResumeId));
                await _context.Database.ExecuteSqlRawAsync(query, companyIdParam, resumeIdParam);
            }
            catch (Exception ex)
            {
                return "此履歷已收藏";
            }


            return "履歷已成功收藏";
        }

        public async Task<string> DeleteFavorite([FromBody] DeleteFavoriteResumesViewModel df)
        {
            try
            {
                var companyIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (companyIdClaim == null)
                {
                    return "無法獲取使用者 ID，請重新登入";
                }
                var companyId = int.Parse(companyIdClaim.Value);
                var query = "DELETE FROM CompanyResumeLikeRecords WHERE CompanyId = @CompanyId AND ResumeId = @ResumeId";
                var companyIdParam = new SqlParameter("@CompanyId", companyId);
                var resumeIdParam = new SqlParameter("@ResumeId", df.ResumeId);
                await _context.Database.ExecuteSqlRawAsync(query, companyIdParam, resumeIdParam);
            }
            catch (Exception ex)
            {
                return "取消收藏履歷失敗";
            }

            return "取消收藏履歷成功";
        }
        [NonAction]
        static int CalculateAge(DateOnly birthday, DateOnly today)
        {
            int age = today.Year - birthday.Year;
            if (today < new DateOnly(today.Year, birthday.Month, birthday.Day))
            {
                age--;
            }
            return age;
        }

        public async Task<FileResult> GetPicture(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string FileName = Path.Combine(webRootPath, "images", "No_Image_Available.jpg");
            Resume c = await _context.Resumes.FindAsync(id);
            byte[] ImageContent = c?.Headshot != null ? c.Headshot : System.IO.File.ReadAllBytes(FileName);
            return File(ImageContent, "image/jpeg");
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
        public async Task<string> Upload([FromForm] CertificationUpLoadViewModel file)
        {
            if (file.FileData == null)
            {
                return "未選擇檔案";
            }
            using (BinaryReader br = new BinaryReader(file.FileData.OpenReadStream()))
            {
                var fileRecord = new ResumeCertification
                {
                    ResumeId = file.ResumeId,
                    CertificationName = file.CertificationName,
                    FileData = br.ReadBytes((int)file.FileData.Length),
                    ContentType = file.ContentType
                };

                _context.ResumeCertifications.Add(fileRecord);
                await _context.SaveChangesAsync();
            }
            return "上傳成功";
        }

        private static void IsPicture(InsterLetter letter, OpinionLetter o)
        {
            if (letter.ImageFile != null)
            {
                using (BinaryReader br = new BinaryReader(letter.ImageFile.OpenReadStream()))
                {
                    o.Attachment = br.ReadBytes((int)letter.ImageFile.Length);
                }
            }
        }
        public string NormalizeAddress(string address)
        {
            return address.Replace("臺", "台");
        }
        public void EditResume(ResumeInputModel resume)
        {
            resume.AreaName = NormalizeAddress(resume.AreaName);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
