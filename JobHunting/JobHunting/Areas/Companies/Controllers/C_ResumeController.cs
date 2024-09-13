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
        public IActionResult ResumeStorageList()
        {
            return View();
        }



        //GET:compaines/C_Resume/ResumeStorageJson
        //[HttpGet]
        //public JsonResult ResumeStorageJson()
        //{
        //    var Candidate = _context.Candidates;
        //    return Json(_context.Resumes.Select(p => new
        //    {
        //        jobTitle = _context.ResumeOpeningRecords.Where(ror => ror.ResumeId == p.ResumeId).Select(ror => ror.OpeningTitle).Single(),
        //        candidateName = Candidate.Where(c => c.CandidateId == p.CandidateId).Select(c => c.Name).Single(),
        //        candidateSex = Candidate.Where(c => c.CandidateId == p.CandidateId).Select(c => c.Sex).Single(),
        //        candidateDegree = Candidate.Where(c => c.CandidateId == p.CandidateId).Select(c => c.Degree).Single(),
        //        candidateEmpStatus = Candidate.Where(c => c.CandidateId == p.CandidateId).Select(c => c.EmploymentStatus).Single(),

        //    }));
        //}
        //GET:compaines/C_Resume/ResumeStorageResult
        [HttpGet]
        public JsonResult ResumeStorageResult()
        {
            
            return Json(_context.Resumes.Include(cd => cd.Companies).Include(c => c.Candidate).Select(n => new
            {
                ResumeId = n.ResumeId,
                CandidateId = n.CandidateId,
                CandidateName = n.Candidate.Name,
                CandidateAddress = n.Address,
                CandidateSex = n.Candidate.Sex,
                CandidateBirthday = n.Candidate.Birthday,
                CandidatePhone = n.Candidate.Phone,
                CandidateDegree = n.Candidate.Degree,
                CandidateEmail = n.Candidate.Email,
                CandidateEmploymentStatus = n.Candidate.EmploymentStatus,
                ResumeTime = n.Time,
                ResumeCertification = n.Certification,
                ResumeWorkExperience = n.WorkExperience,
                ResumeAutobiography = n.Autobiography,
                jobTitle = _context.ResumeOpeningRecords.Where(ror => ror.ResumeId == n.ResumeId).Select(ror => ror.OpeningTitle).Single(),
                ResumeTitle = n.Title,
                ResumeIntro = n.Intro,
                ResumeTitleClassId = n.TitleClasses.Select(rtc => rtc.TitleClassId),
                ResumeTitleClassName = n.TitleClasses.Select(rtc => rtc.TitleClassName),
                ResumeTagId = n.Tags.Select(t => t.TagId),
                ResumeTagName = n.Tags.Select(t => t.TagName),
                ResumeHeadshot = n.Headshot != null ? Convert.ToBase64String(n.Headshot) : null,
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
