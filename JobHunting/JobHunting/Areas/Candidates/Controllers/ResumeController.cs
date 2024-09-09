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
                titleClass = a.TitleClasses,
                edit = false,
            }));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReasumes ([FromBody]addResumeInputModel Creatr)
        {
            //if (ModelState.IsValid)
            //{

            try
            {
                var candidate = await _context.Candidates.FindAsync(Creatr.CandidateId);

                if (candidate == null)
                {
                    return NotFound(new { Message = "Resume not found" });
                }

                Resume insert = new Resume()
                {
                    Address = Creatr.Address,
                    //Time = Creatr.Time,
                    Title = Creatr.Title,
                    //Certification = Creatr.Certification,
                    WorkExperience = Creatr.WorkExperience,
                    Autobiography = Creatr.Autobiography,
                    ReleaseYN = Creatr.ReleaseYN,
                    CandidateId = Creatr.CandidateId,
                    Intro = Creatr.Intro,
                };
                _context.Resumes.Add(insert);
                await _context.SaveChangesAsync();
                
            }
            catch (Exception ex) 
            {
                return Json(new { message = "新增履歷失敗" });
            }
            return Json(new { success = true, message = "新增履歷成功" });
            //candidate.Name = Creatr.Name;
            //candidate.Sex = Creatr.Sex;
            //candidate.Birthday = Creatr.Birthday;
            //candidate.Phone = Creatr.Phone;
            //candidate.Degree = Creatr.Degree;
            //candidate.Email = Creatr.Email;
            //candidate.EmploymentStatus = Creatr.EmploymentStatus;


        }
            //return View(Creatr);
        //}

        //Post: /Candidates/Resume/EditResume
        [HttpPost]
        public async Task<IActionResult> EditResume ([FromBody][Bind("Address", "Title", "Autobiography", "WorkExperience", "Time", "ReleaseYN")] ResumeInputModel rm)
        {
            

            var r = await _context.Resumes.FindAsync(rm.ResumeId);

            if (r == null)
            {
                return NotFound(new { Message = "Resume not found" });
            }

            //c.Name = rm.Name;
            //c.Sex = rm.Sex;
            //c.Birthday = rm.Birthday;
            //c.EmploymentStatus = rm.EmploymentStatus;
            //c.Phone = rm.Phone;
            //c.Degree = rm.Degree;
            //c.Email = rm.Email;
            r.Intro = rm.Intro;
            r.Address = rm.Address;
            r.Title = rm.Title;
            r.Autobiography = rm.Autobiography;
            r.WorkExperience = rm.WorkExperience;
            r.Time = rm.Time;
            r.ReleaseYN = rm.ReleaseYN;

            //_context.Entry(r).State = EntityState.Modified;
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
