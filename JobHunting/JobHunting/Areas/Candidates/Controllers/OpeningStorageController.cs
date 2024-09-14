using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Area("Candidates")]
    public class OpeningStorageController : Controller
    {
        private readonly DuckCandidatesContext _context;


        public OpeningStorageController(DuckCandidatesContext context)
        {
            _context = context;
        }
        public IActionResult OpeningStorage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IEnumerable<CandidateOpeningStorageOutputModel>> CandidateOpenings([FromBody] int id)
        {
            return _context.ResumeOpeningRecords.Include(o=>o.Opening)
                .Where(ror => ror.Resume.CandidateId == id)
                .Select(ror => new CandidateOpeningStorageOutputModel
                {
                    OpeningId = ror.OpeningId,
                    Title = ror.OpeningTitle,
                    CompanyName = ror.CompanyName,
                    Address = ror.Opening.Address,
                    ContactName = ror.Opening.ContactName,
                    ContactPhone = ror.Opening.ContactPhone,
                    ContactEmail = ror.Opening.ContactEmail,
                    SalaryMax = ror.Opening.SalaryMax,
                    SalaryMin = ror.Opening.SalaryMin,
                    Time = ror.Opening.Time,
                    Description = ror.Opening.Description,
                    TitleClassId = ror.Opening.TitleClasses.Select(tc => tc.TitleClassId).ToList(),
                    TagId = ror.Opening.Tags.Select(t => t.TagId).ToList(),
                    Benefits = ror.Opening.Benefits,
                    Degree = ror.Opening.Degree,
                });
        }
        //GET:Companies/Openings/TitleClassJson
        [HttpGet]
        public JsonResult TitleClassJson()
        {
            return Json(_context.TitleClasses.Select(tc => new
            {
                TitleClassId = tc.TitleClassId,
                TitleClassName = tc.TitleClassName,
                TitleCategoryId = tc.TitleCategoryId
            }));
        }
        //GET:Companies/Openings/TitleCategoryJosn
        [HttpGet]
        public JsonResult TitleCategoryJson()
        {
            return Json(_context.TitleCategories.Select(tc => new
            {
                TitleCategoryId = tc.TitleCategoryId,
                TitleCategoryName = tc.TitleCategoryName
            }));
        }
        //GET:Compaines/Openings/TagsJson
        [HttpGet]
        public JsonResult TagJson()
        {
            return Json(_context.Tags.Select(t => new
            {
                TagId = t.TagId,
                TagName = t.TagName,
                TagClassId = t.TagClassId
            }));
        }
        //GET:Companies/Openings/TagClassJson
        [HttpGet]
        public JsonResult TagClassJson()
        {
            return Json(_context.TagClasses.Select(tc => new
            {
                TagClassId = tc.TagClassId,
                TagClassName = tc.TagClassName
            }));
        }
    }
}
