using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        //GET:compaines/C_Resume/ResumeStorageJson
        [HttpGet]
        public JsonResult ResumeStorageJson()
        {
            var Candidate = _context.Candidates;
            return Json(_context.Resumes.Select(p => new
            {
                jobTitle = _context.ResumeOpeningRecords.Where(ror => ror.ResumeId == p.ResumeId).Select(ror => ror.OpeningTitle).Single(),
                candidateName = Candidate.Where(c => c.CandidateId == p.CandidateId).Select(c => c.Name).Single(),
                candidateSex = Candidate.Where(c => c.CandidateId == p.CandidateId).Select(c => c.Sex).Single(),
                candidateDegree = Candidate.Where(c => c.CandidateId == p.CandidateId).Select(c => c.Degree).Single(),
                candidateEmpStatus = Candidate.Where(c => c.CandidateId == p.CandidateId).Select(c => c.EmploymentStatus).Single(),

            }));
        }
        //GET:Compaines/C_Resume/ReceiveResume
        [HttpGet]
        public IActionResult ReceiveResume()
        {
            return View();
        }
        //GET:compaines/C_Resume/ReceiveResumeJson
        [HttpGet]
        public JsonResult ReceiveResumeJson()
        {
            var Openings = _context.Openings;
            var ResumeOpeningRecords = _context.ResumeOpeningRecords;
            return Json(Openings.Select(p => new
            {
                Title = p.Title,
                TitleId = p.OpeningId,
                TitleClassName = p.TitleClasses.Select(tc => tc.TitleClassName).FirstOrDefault(),
                TitleClassId = p.TitleClasses.Select(tc => tc.TitleClassId).FirstOrDefault(),
                ApplyDate = p.ResumeOpeningRecords.Select(ror => ror.ApplyDate).FirstOrDefault(),
            }));
        }
    }

}
