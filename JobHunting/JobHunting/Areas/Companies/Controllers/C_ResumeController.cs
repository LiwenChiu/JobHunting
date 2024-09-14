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

        [HttpPost]
        public async Task<IEnumerable<ResumeStorageViewModel>> ResumeStorageJson( int id)
            
        {

            var Result = await _context.Companies.Include(i => i.Resumes).ThenInclude(ti => ti.Tags)
                          .Include(i => i.Resumes).ThenInclude(ti => ti.TitleClasses)
                          .Include(i => i.Resumes).ThenInclude(ti => ti.Candidate)
                          .Include(i=>i.Openings).ThenInclude(ti=>ti.ResumeOpeningRecords)
                          .FirstOrDefaultAsync(rs => rs.CompanyId == id);
             if(Result == null)
            {
                return new List<ResumeStorageViewModel>();
            }
            var companies = Result.Resumes.Select(n => new ResumeStorageViewModel
            {
                Name = n.Candidate.Name,
                Address = n.Candidate.Address,
                Sex = n.Candidate.Sex,
                Birthday = n.Candidate.Birthday,
                Phone = n.Candidate.Phone,
                Degree = n.Candidate.Degree,
                Email = n.Candidate.Email,
                EmploymentStatus = n.Candidate.EmploymentStatus,
                Time = n.Time,
                ResumeId = n.ResumeId,
                CandidateId = n.CandidateId,
                Certification = n.Certification, /*!= null ? Convert.ToBase64String(n.Certification) : null,*/
                WorkExperience = n.WorkExperience,
                Autobiography = n.Autobiography,
                OpeningTitle = _context.ResumeOpeningRecords.Where(ror => ror.ResumeId == n.ResumeId).Select(ror => ror.OpeningTitle).Single(),
                Intro = n.Intro,
                TitleClassId = n.TitleClasses.Select(rtc => rtc.TitleClassId).ToList(),
                TagId = n.Tags.Select(t => t.TagId).ToList(),
                Headshot = n.Headshot, /*!= null ? Convert.ToBase64String(n.Headshot) : null,*/
            });



            return companies;
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


        //[HttpPost]
        //public async Task<IActionResult> 







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
