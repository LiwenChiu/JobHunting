using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Authorize(Roles = "candidate")]
    [Area("Candidates")]
    public class ResumeController : Controller
    {
        private readonly DuckCandidatesContext _context;
        private readonly IWebHostEnvironment _environment;

        public ResumeController(DuckCandidatesContext context , IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }


        //GET: /Candidates/Resume/ResumeResult
        [HttpGet]
        public async Task<JsonResult> CandidatesResumeResult()
        {
            var CandidateId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(CandidateId))
            {
                return Json(new { message = "未授權訪問" });
            }

            return Json(_context.Resumes.Include(r => r.Candidate).Include(s => s.ResumeCertifications).Where(w=>w.CandidateId.ToString() == CandidateId).Include(t => t.TitleClasses).Select(a => new
            {
                name = a.Candidate.Name,
                address = a.Address,
                sex = a.Candidate.Sex,
                birthday = a.Candidate.Birthday,
                phone = a.Candidate.Phone,
                degree = a.Candidate.Degree,
                email = a.Candidate.Email,
                employmentStatus = a.Candidate.EmploymentStatus,
                time = a.Time,
                title = a.Title,
                certification = a.ResumeCertifications.Select(a => a.CertificationName),
                workExperience = a.WorkExperience,
                autobiography = a.Autobiography,
                candidateid = int.Parse(CandidateId),
                resumeid = a.ResumeId,
                releaseYN = a.ReleaseYN,
                intro = a.Intro,
                TitleClassId = a.TitleClasses.Select(rtc => rtc.TitleClassId),
                TitleClassName = a.TitleClasses.Select(rtc => rtc.TitleClassName),
                TagId = a.Tags.Select(t => t.TagId),
                TagName = a.Tags.Select(t => t.TagName),
                headshot = a.Headshot != null ? Convert.ToBase64String(a.Headshot) : null,
                FileName = a.ResumeCertifications.Select(z => new { z.ResumeId, z.CertificationId, z.CertificationName }),
                edit = false,
                ReleaseYNedit = false,
                LastEditTime = a.LastEditTime,
            }).OrderByDescending(a => a.LastEditTime));
        }
        //新增履歷的求職者基本資料
        [HttpGet]
        public async Task<IActionResult> Candidateinformation()
        {
            var CandidateId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(CandidateId))
            {
                return Json(new { message = "未授權訪問" });
            }

            return Json(_context.Candidates.Where(r=>r.CandidateId.ToString() == CandidateId).Select(a => new
            {
                name = a.Name,
                sex = a.Sex,
                birthday = a.Birthday,
                phone = a.Phone,
                degree = a.Degree,
                email = a.Email,
                employmentStatus = a.EmploymentStatus,
            }));

        }

        //撈全部的履歷資料
        //{

        //    return Json(_context.Resumes.Include(r => r.Candidate).Include(t => t.TitleClasses).Select(a => new
        //    {
        //        name = a.Candidate.Name,
        //        address = a.Address,
        //        sex = a.Candidate.Sex,
        //        birthday = a.Candidate.Birthday,
        //        phone = a.Candidate.Phone,
        //        degree = a.Candidate.Degree,
        //        email = a.Candidate.Email,
        //        employmentStatus = a.Candidate.EmploymentStatus,
        //        time = a.Time,
        //        title = a.Title,
        //        certification = a.Certification,
        //        workExperience = a.WorkExperience,
        //        autobiography = a.Autobiography,
        //        candidateid = a.CandidateId,
        //        resumeid = a.ResumeId,
        //        releaseYN = a.ReleaseYN,
        //        intro = a.Intro,
        //        TitleClassId = a.TitleClasses.Select(rtc => rtc.TitleClassId),
        //        TitleClassName = a.TitleClasses.Select(rtc => rtc.TitleClassName),
        //        TagId = a.Tags.Select(t => t.TagId),
        //        TagName = a.Tags.Select(t => t.TagName),
        //        headshot = a.Headshot != null ? Convert.ToBase64String(a.Headshot) : null,
        //        edit = false,
        //    }));
        //}

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
        //GET:Candidates/Resume/TagClassJson
        [HttpGet]
        public JsonResult TagClassJson()
        {
            return Json(_context.TagClasses.Select(rtc => new
            {
                TagClassId = rtc.TagClassId,
                TagClassName = rtc.TagClassName
            }));
        }

        //GET: Candidates/Resume/GetCandidateData
        public async Task<GetCandidateDataOutputViewModel> GetCandidateData()
        {
            var candidateIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(candidateIdClaim) || !int.TryParse(candidateIdClaim, out int candidateId))
            {
                return new GetCandidateDataOutputViewModel { AlertText = "失敗" };
            }

            var candidateData = await _context.Candidates.AsNoTracking()
                .Where(c => c.CandidateId == candidateId)
                .Select(c => new GetCandidateDataInputViewModel
                {
                    Name = c.Name,
                    Sex = c.Sex,
                    Birthday = c.Birthday,
                    Phone = c.Phone,
                    Address = c.Address,
                    Degree = c.Degree,
                    VerifyEmailYN = c.VerifyEmailYN,
                }).FirstOrDefaultAsync();

            if (candidateData == null)
            {
                return new GetCandidateDataOutputViewModel { AlertText = "失敗" };
            }

            if (!candidateData.VerifyEmailYN)
            {
                return new GetCandidateDataOutputViewModel { DataStatus = false, AlertText = "驗證信箱" };
            }

            if (string.IsNullOrEmpty(candidateData.Name) || !candidateData.Sex.HasValue || !candidateData.Birthday.HasValue || string.IsNullOrEmpty(candidateData.Phone) || string.IsNullOrEmpty(candidateData.Address) || string.IsNullOrEmpty(candidateData.Degree))
            {
                return new GetCandidateDataOutputViewModel { DataStatus = false, AlertText = "完整填寫會員資料" };
            }

            return new GetCandidateDataOutputViewModel { DataStatus = true, AlertText = "新增履歷" };
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReasumes ([FromForm] addResumeInputModel Creatr)
        {
            var candidateIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(candidateIdClaim) || !int.TryParse(candidateIdClaim, out int candidateId))
            {
                return Unauthorized("未授權訪問");
            }

            if (Creatr.Title ==null)
            {
                return Json(new { success = false, message = "履歷名稱未填寫" });
            }


            try
            {
                var candidate = await  _context.Candidates.FindAsync(candidateId);

                if (candidate == null)
                {
                    return NotFound(new { Message = "Resume not found" });
                }

                if(candidate.Name == null || candidate.Sex == null || candidate.Birthday == null || candidate.Phone == null)
                {
                    return RedirectToAction("Index", "Member", new { area = "Candidates" });
                }

                var titleClasses = await _context.TitleClasses
                   .Where(tc => Creatr.TitleClassId.Contains(tc.TitleClassId))
                   .ToListAsync();

                var tags = await _context.Tags
                    .Where(t => Creatr.TagId.Contains(t.TagId))
                    .ToListAsync();

                Resume insert = new Resume()
                {
                    Address = Creatr.Address,
                    Time = Creatr.Time,
                    Title = Creatr.Title,
                    WorkExperience = Creatr.WorkExperience,
                    Autobiography = Creatr.Autobiography,
                    ReleaseYN = Creatr.ReleaseYN,
                    CandidateId = candidateId,
                    Intro = Creatr.Intro,
                    LastEditTime = DateTime.Now,
                };
                if (Creatr.HeadshotImageFile != null)
                {
                    using (BinaryReader br = new BinaryReader(Creatr.HeadshotImageFile.OpenReadStream()))
                    {
                        insert.Headshot = br.ReadBytes((int)Creatr.HeadshotImageFile.Length);
                    }
                }

                if (Creatr.CertificationFile != null)
                {
                    using (BinaryReader br = new BinaryReader(Creatr.CertificationFile.OpenReadStream()))
                    {
                        var fileRecord = new ResumeCertification
                        {
                            CertificationName = Creatr.CertificationName,
                            FileData = br.ReadBytes((int)Creatr.CertificationFile.Length),
                            ContentType = Creatr.CertificationType,
                        };
                        insert.ResumeCertifications.Add(fileRecord);
                    }
                }

                foreach (var titleClass in titleClasses)
                {
                    insert.TitleClasses.Add(titleClass);
                }

                foreach (var tag in tags)
                {
                    insert.Tags.Add(tag);
                }


                
                //var resumeId = insert.ResumeId;
                //List<string> uploadedFilePaths = new List<string>();
                ////在wwwroot的images根據求職者的名稱與履歷id建立資料夾
                //var candidateFolderName = $"{resumeId}";
                //var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", candidateFolderName);

                //// 如果資料夾不存在，就建立資料夾
                //if (!Directory.Exists(uploadsFolderPath))
                //{
                //    Directory.CreateDirectory(uploadsFolderPath);
                //}

                //// 保存文件
                //foreach (var formFile in Creatr.CertificationImageFile)
                //{
                //    if (formFile.Length > 0)
                //    {
                //        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                //        var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                //        // 将文件保存到指定路径
                //        using (var stream = new FileStream(filePath, FileMode.Create))
                //        {
                //            await formFile.CopyToAsync(stream);
                //        }

                //        // 保存文件路径或文件名，以便返回前端
                //        uploadedFilePaths.Add($"/images/{candidateFolderName}/{uniqueFileName}");
                //    }
                //}

                // 將 Resume 實體加入資料庫上下文
                _context.Resumes.Add(insert);

                // 儲存變更並將 ResumeId 自動生成
                await _context.SaveChangesAsync();
 

            }
            catch (Exception ex) 
            {
                return Json(new { success = false, message = "新增履歷失敗" });
            }
            //return Json(new { success = true, message = "新增履歷成功" });

            return Json(new { success = true, message = "新增履歷成功",});

        }
        //return View(Creatr);
        //}


        //Post: /Candidates/Resume/EditResume   //修改履歷狀態
        [HttpPost]
        public async Task<IActionResult> EditReleaseYN([FromBody][Bind("ReleaseYN", "ResumeId")] ResumeInputModel rm)
        {
            


            var r = await _context.Resumes.FindAsync(rm.ResumeId);

            if (r == null)
            {
                return NotFound(new { Message = "Resume not found" });
            }

            r.ReleaseYN = rm.ReleaseYN;


            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return Json(new { success = false,message = "履歷狀態更改失敗" });
            }

            return Json(new { success = true, message = "履歷狀態已更改" });
        }


        [HttpPost]
        public async Task<IActionResult> EditResume([FromForm][Bind("Address", "Title", "Autobiography", "WorkExperience", "Time", "ReleaseYN", "ResumeId", "HeadshotImageFile", "TitleClassId", "TagId", "Intro", "CertificationFile", "CertificationName", "CertificationType")] ResumeInputModel rm)
        {

            try
            {
                if (rm.Title == null)
                {
                    return Json(new { success = false, message = "履歷名稱不能空白" });
                }

                var r = await _context.Resumes
            .Include(o => o.TitleClasses).Include(t => t.Tags).Include(s => s.ResumeCertifications)
            .FirstOrDefaultAsync(o => o.ResumeId == rm.ResumeId);

                var titleClasses = await _context.TitleClasses
                       .Where(tc => rm.TitleClassId.Contains(tc.TitleClassId))
                       .ToListAsync();

                var tags = await _context.Tags
                    .Where(t => rm.TagId.Contains(t.TagId))
                    .ToListAsync();

                //Resume r = await _context.Resumes.FindAsync(rm.ResumeId);

                r.Intro = rm.Intro;
                r.Address = rm.Address;
                r.Title = rm.Title;
                r.Autobiography = rm.Autobiography;
                r.WorkExperience = rm.WorkExperience;
                r.Time = rm.Time;
                r.ReleaseYN = rm.ReleaseYN;
                r.TitleClasses.Clear();
                r.Tags.Clear();
                r.LastEditTime = DateTime.Now;

                if (rm.HeadshotImageFile != null)
                {
                    using (BinaryReader br = new BinaryReader(rm.HeadshotImageFile.OpenReadStream()))
                    {
                        r.Headshot = br.ReadBytes((int)rm.HeadshotImageFile.Length);
                    }
                }
                if (rm.CertificationFile != null)
                {
                    using (BinaryReader br = new BinaryReader(rm.CertificationFile.OpenReadStream()))
                    {
                        var fileRecord = new ResumeCertification
                        {
                            ResumeId = rm.ResumeId,
                            CertificationName = rm.CertificationName,
                            FileData = br.ReadBytes((int)rm.CertificationFile.Length),
                            ContentType = rm.CertificationType,
                        };
                        _context.ResumeCertifications.Add(fileRecord);
                    }
                }


                foreach (var titleClass in titleClasses)
                {
                    r.TitleClasses.Add(titleClass);
                }

                foreach (var tag in tags)
                {
                    r.Tags.Add(tag);
                }



                _context.Update(r);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return Json(new { success = false, message = "修改履歷失敗" });
            }
            return Json(new { success = true, message = "修改履歷成功" });

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








        [HttpDelete]
        public async Task<IActionResult> DelResume([FromBody] ResumeInputModel rm)
        {
            var resume = await _context.Resumes.FindAsync(rm.ResumeId);
            if (resume != null)
            {
                _context.Resumes.Remove(resume);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) 
            {
                return Json(new { success = false,message = "刪除履歷失敗" });
            }
            return Json(new { success = true,message = "刪除履歷成功" });
        }
        public async Task<IActionResult> DelCertification(int id)
        {
            var certification = await _context.ResumeCertifications.FindAsync(id);
            if (certification != null)
            {
                _context.ResumeCertifications.Remove(certification);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return Json(new { success = false, message = "刪除檔案失敗" });
            }
            return Json(new { success = true, message = "刪除檔案成功" });
        }
    }


    
}
