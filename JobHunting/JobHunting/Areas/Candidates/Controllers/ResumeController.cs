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


            return Json(_context.Resumes.Include(r => r.Candidate).Select(a => new
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
                edit = false,
            }));
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateReasumes(addResumeInputModel Creatr)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var candidate = await _context.Candidates.FindAsync(Creatr.CandidateId);

        //        if (candidate == null)
        //        {
        //            return NotFound();
        //        }

        //        Resume insert = new Resume()
        //        {
        //            Address = Creatr.Address,
        //            Time = Creatr.Time,
        //            Title = Creatr.Title,
        //            Certification = Creatr.Certification,
        //            WorkExperience = Creatr.WorkExperience,
        //            Autobiography = Creatr.Autobiography,
        //            ReleaseYN = Creatr.ReleaseYN,
        //            CandidateId = Creatr.CandidateId,
        //            ResumeId = Creatr.ResumeId,
        //        };

        //        //candidate.Name = Creatr.Name;
        //        //candidate.Sex = Creatr.Sex;
        //        //candidate.Birthday = Creatr.Birthday;
        //        //candidate.Phone = Creatr.Phone;
        //        //candidate.Degree = Creatr.Degree;
        //        //candidate.Email = Creatr.Email;
        //        //candidate.EmploymentStatus = Creatr.EmploymentStatus;




        //        _context.Resumes.Add(insert);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(ResumeManage));
        //    }
        //    return View(Creatr);
        //}

        //Post: /Candidates/Resume/EditResume
        [HttpPost]
        public async Task<string> EditResume([FromBody] ResumeInputModel rm)
        {


            var r = await _context.Resumes.FindAsync(rm.ResumeId);
            

            //c.Name = rm.Name;
            //c.Sex = rm.Sex;
            //c.Birthday = rm.Birthday;
            //c.EmploymentStatus = rm.EmploymentStatus;
            //c.Phone = rm.Phone;
            //c.Degree = rm.Degree;
            //c.Email = rm.Email;
            r.Address = rm.Address;
            r.Title = rm.Title;
            r.Autobiography = rm.Autobiography;
            r.WorkExperience = rm.WorkExperience;
            r.Time = rm.Time;
            r.ReleaseYN = rm.ReleaseYN;

            //_context.Entry(r).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                  return "修改成功";
            
        }









    }
}
