using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
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

        public IActionResult CreateReasume()
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
                candidateid = a.CandidateID
            }));
        }

        //[HttpPost]

        //public async Task<IActionResult> CreateReasumes (addResumeViewModel Creatr)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        addResumeViewModel insert = new addResumeViewModel()
        //        {
        //            Name = Creatr.Name,
        //            Address = Creatr.Address,
        //            Sex = Creatr.Sex,
        //            Birthday = Creatr.Birthday,
        //            Phone = Creatr.Phone,
        //            Degree = Creatr.Degree,
        //            Email = Creatr.Email,
        //            EmploymentStatus = Creatr.EmploymentStatus,
        //            Time = Creatr.Time,
        //            Title = Creatr.Title,
        //            TitleClassID = Creatr.TitleClassID,
        //            Certification = Creatr.Certification,
        //            WorkExperience = Creatr.WorkExperience,
        //            Autobiography = Creatr.Autobiography,
        //            CandidateID = Creatr.CandidateID
        //        };
        //        _context.
        //    }
        }










        //[HttpPost]

        //public async Task<IActionResult> CandidatesResumeDetail(int backid)
        //{
        //   var member = await _context.Candidates.Where(c => c.CandidateID == backid).FirstOrDefaultAsync();


        //    if(member == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(member);


        //}
    //}
}
