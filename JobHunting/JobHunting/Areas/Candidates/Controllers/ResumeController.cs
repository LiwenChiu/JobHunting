using JobHunting.Areas.Candidates.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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


        //POST: /Candidates/Resume/ResumeResult
        [HttpPost]
        public JsonResult ResumeResult()
        {


            return Json(_context.Resumes.Include(r => r.Candidate).Select(a => new
            {
                name = a.Candidate.Name,
                CandidateAddress = a.Candidate.Address,
                Sex = a.Candidate.Sex,
                birthday = a.Candidate.Birthday,
                phone = a.Candidate.Phone,
                Degree = a.Candidate.Degree,
                Email = a.Candidate.Email,
                EmploymentStatus = a.Candidate.EmploymentStatus,
                HopeTime = a.Time,
                ResumeTitle = a.Title,
                TitleClassID = a.TitleClassID,
                Certification = a.Certification,
                WorkExperience = a.WorkExperience,
                Autobiography = a.Autobiography,
            }));
        }
    }
}
