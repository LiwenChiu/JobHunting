using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Protocol.Plugins;
using System.Security.Cryptography;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Area("Candidates")]
    public class ResumeController : Controller
    {
        private readonly DuckCandidatesContext _context;


        public ResumeController(DuckCandidatesContext context)
        {
            _context = context;
        }
        public IActionResult ResumeManage()
        {
            return View();
        }


        //GET: /Candidates/Resume/ResumeResult
        [HttpGet]
        public async Task<JsonResult> CandidatesResumeResult(int id)
        {

            return Json(_context.Resumes.Include(r => r.Candidate).Where(w=>w.CandidateId == id).Include(t => t.TitleClasses).Select(a => new
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
                certification = a.Certification,
                workExperience = a.WorkExperience,
                autobiography = a.Autobiography,
                candidateid = a.CandidateId,
                resumeid = a.ResumeId,
                releaseYN = a.ReleaseYN,
                intro = a.Intro,
                TitleClassId = a.TitleClasses.Select(rtc => rtc.TitleClassId),
                TitleClassName = a.TitleClasses.Select(rtc => rtc.TitleClassName),
                TagId = a.Tags.Select(t => t.TagId),
                TagName = a.Tags.Select(t => t.TagName),
                headshot = a.Headshot != null ? Convert.ToBase64String(a.Headshot) : null,
                edit = false,
                ReleaseYNedit = false,
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
        //GET:Companies/Openings/TagClassJson
        [HttpGet]
        public JsonResult TagClassJson()
        {
            return Json(_context.TagClasses.Select(rtc => new
            {
                TagClassId = rtc.TagClassId,
                TagClassName = rtc.TagClassName
            }));
        }








        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReasumes ([FromForm] addResumeInputModel Creatr)
        {
            //if (ModelState.IsValid)
            //{
           

            try
            {
                var candidate = await  _context.Candidates.FindAsync(Creatr.CandidateId);

                if (candidate == null)
                {
                    return NotFound(new { Message = "Resume not found" });
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
                    CandidateId = Creatr.CandidateId,
                    Intro = Creatr.Intro,
                   
                };
                if (Creatr.HeadshotImageFile != null)
                {
                    using (BinaryReader br = new BinaryReader(Creatr.HeadshotImageFile.OpenReadStream()))
                    {
                        insert.Headshot = br.ReadBytes((int)Creatr.HeadshotImageFile.Length);
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

                _context.Resumes.Add(insert);
                await _context.SaveChangesAsync();

                //var resumeId = insert.ResumeId;
                //List<string> uploadedFilePaths = new List<string>();
                ////在wwwroot的images根據求職者的名稱與履歷id建立資料夾
                //var candidateFolderName = $"{candidate.Name}_{resumeId}";
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

                //return Json(new { success = true, message = "新增履歷成功", files = uploadedFilePaths });

            }
            catch (Exception ex) 
            {
                return Json(new { message = "新增履歷失敗" });
            }
            return Json(new { success = true, message = "新增履歷成功" });



        }
        //return View(Creatr);
        //}

        //Post: /Candidates/Resume/EditResume
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
                return Json(new { message = "修改失敗" });
            }

            return Json(new { success = true, message = "修改成功" });
        }


        [HttpPost]
        public async Task<IActionResult> EditResume([FromForm][Bind("Address", "Title", "Autobiography", "WorkExperience", "Time", "ReleaseYN", "ResumeId", "HeadshotImageFile", "TitleClassId", "TagId", "Intro")] ResumeInputModel rm)
        {
            

            var r = await _context.Resumes
            .Include(o => o.TitleClasses).Include(t => t.Tags)
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

            if (rm.HeadshotImageFile != null)
            {
                using (BinaryReader br = new BinaryReader(rm.HeadshotImageFile.OpenReadStream()))
                {
                    r.Headshot = br.ReadBytes((int)rm.HeadshotImageFile.Length);
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
            return Json(new { success = true, message = "修改成功" });

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
                return Json(new { message = "刪除履歷失敗" });
            }
            return Json(new { success = true,message = "刪除履歷成功" });
        }

    }


    
}
