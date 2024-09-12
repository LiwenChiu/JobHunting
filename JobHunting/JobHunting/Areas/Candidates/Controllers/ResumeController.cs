using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Protocol.Plugins;

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
        public JsonResult ResumeResult()
        {


            return Json(_context.Resumes.Include(r => r.Candidate).Include(t=>t.TitleClasses).Select(a => new
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
                //titleClass = a.TitleClasses,
                headshot = a.Headshot != null ? Convert.ToBase64String(a.Headshot) : null,
                edit = false,
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

                Resume insert = new Resume()
                {
                    Address = Creatr.Address,
                    Time = Creatr.Time,
                    Title = Creatr.Title,
                    WorkExperience = Creatr.WorkExperience,
                    Autobiography = Creatr.Autobiography,
                    ReleaseYN = Creatr.ReleaseYN,
                    //CandidateId = Creatr.CandidateId,
                    Intro = Creatr.Intro,
                   
                };
                if (Creatr.HeadshotImageFile != null)
                {
                    using (BinaryReader br = new BinaryReader(Creatr.HeadshotImageFile.OpenReadStream()))
                    {
                        insert.Headshot = br.ReadBytes((int)Creatr.HeadshotImageFile.Length);
                    }
                }

                //foreach (var formFile in Creatr.CertificationImageFile)
                //{
                //    if (formFile.Length > 0)
                //    {// 請替換為你希望保存文件的實際路徑，這裡假設是 wwwroot/uploads
                //        var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                //        // 如果目錄不存在，則創建目錄
                //        if (!Directory.Exists(uploadsFolderPath))
                //        {
                //            Directory.CreateDirectory(uploadsFolderPath);
                //        }

                //        // 生成唯一文件名，防止重名文件被覆蓋
                //        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

                //        // 獲取完整的文件保存路徑
                //        var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                //        // 將文件保存到指定路徑
                //        using (var stream = new FileStream(filePath, FileMode.Create))
                //        {
                //            await formFile.CopyToAsync(stream);
                //        }
                //    }
                //}

                _context.Resumes.Add(insert);
                await _context.SaveChangesAsync();
                
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
        //[HttpPost]
        //public async Task<IActionResult> EditResume ([FromBody][Bind("Address", "Title", "Autobiography", "WorkExperience", "Time", "ReleaseYN")] ResumeInputModel rm)
        //{
            

        //    var r = await _context.Resumes.FindAsync(rm.ResumeId);

        //    if (r == null)
        //    {
        //        return NotFound(new { Message = "Resume not found" });
        //    }

        //    //c.Name = rm.Name;
        //    //c.Sex = rm.Sex;
        //    //c.Birthday = rm.Birthday;
        //    //c.EmploymentStatus = rm.EmploymentStatus;
        //    //c.Phone = rm.Phone;
        //    //c.Degree = rm.Degree;
        //    //c.Email = rm.Email;
        //    r.Intro = rm.Intro;
        //    r.Address = rm.Address;
        //    r.Title = rm.Title;
        //    r.Autobiography = rm.Autobiography;
        //    r.WorkExperience = rm.WorkExperience;
        //    r.Time = rm.Time;
        //    r.ReleaseYN = rm.ReleaseYN;
        //    r.Headshot = rm.Headshot;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
                
        //    }
        //    catch (Exception ex) 
        //    {
        //        return Json(new { message = "修改失敗" });
        //    }

        //    return Json(new { success = true, message = "修改成功" });
        //}


        [HttpPost]
        public async Task<IActionResult> EditResume([FromForm][Bind("Address", "Title", "Autobiography", "WorkExperience", "Time", "ReleaseYN", "ResumeId", "ImageFile")] ResumeInputModel rm)
        {

            Resume r = await _context.Resumes.FindAsync(rm.ResumeId);

                r.Intro = rm.Intro;
                r.Address = rm.Address;
                r.Title = rm.Title;
                r.Autobiography = rm.Autobiography;
                r.WorkExperience = rm.WorkExperience;
                r.Time = rm.Time;
                r.ReleaseYN = rm.ReleaseYN;

            if (rm.ImageFile != null)
            {
                using (BinaryReader br = new BinaryReader(rm.ImageFile.OpenReadStream()))
                {
                    r.Headshot = br.ReadBytes((int)rm.ImageFile.Length);
                }
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
