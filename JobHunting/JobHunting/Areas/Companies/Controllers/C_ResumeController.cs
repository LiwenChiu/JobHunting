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
        public JsonResult ResumeStorageResult(int id)
        {


            var result = from cd in _context.Candidates
                         join r in _context.Resumes on cd.CandidateId equals r.CandidateId
                         join ror in _context.ResumeOpeningRecords on r.ResumeId equals ror.ResumeId
                         join op in _context.Openings on ror.OpeningId equals op.OpeningId
                         join cp in _context.Companies on op.CompanyId equals cp.CompanyId
                         where cp.CompanyId == id
                         select new
                         {
                             Name = cd.Name,
                             Address = cd.Address,
                             Sex = cd.Sex,
                             Birthday = cd.Birthday,
                             Phone = cd.Phone,
                             Degree = cd.Degree,
                             Email = cd.Email,
                             EmploymentStatus = cd.EmploymentStatus,
                             Time = r.Time,
                             Certification = r.Certification != null ? Convert.ToBase64String(r.Headshot) : null,
                             WorkExperience = r.WorkExperience,
                             Autobiography = r.Autobiography,
                             jobTitle = ror.OpeningTitle,
                             Intro = r.Intro,
                             TitleClassId = r.TitleClasses.Select(rtc => rtc.TitleClassId),
                             TitleClassName = r.TitleClasses.Select(rtc => rtc.TitleClassName),
                             TagId = r.Tags.Select(t => t.TagId),
                             TagName = r.Tags.Select(t => t.TagName),
                             Headshot = r.Headshot != null ? Convert.ToBase64String(r.Headshot) : null,

                         };





            return Json(result);
            
        }

        [HttpGet]
        public JsonResult ResumeStorageJson()
        {

            return Json(_context.Resumes.Include(i=>i.Companies).Include(c => c.Candidate).Include(t => t.TitleClasses).Select(n => new
            {
                
            }));

        }



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
        //GET:Companies/C_Resume/TagClassJson
        [HttpGet]
        public JsonResult TagClassJson()
        {
            return Json(_context.TagClasses.Select(rtc => new
            {
                TagClassId = rtc.TagClassId,
                TagClassName = rtc.TagClassName
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
