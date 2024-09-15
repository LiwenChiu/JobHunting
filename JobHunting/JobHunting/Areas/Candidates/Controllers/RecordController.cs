using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Area("Candidates")]
    public class RecordController : Controller
    {
        private readonly DuckCandidatesContext _context;


        public RecordController(DuckCandidatesContext context)
        {
            _context = context;
        }

        public IActionResult RecordHistory()
        {
            return View();
        }

        /*-------------------------------------------------*/

        //Post: Candidates/Record/Recordfilter
        [HttpPost]
        public async Task<IEnumerable<RecordOutputmodel>> filter([FromBody] RecordViewmodel rv)
        {
            var query = _context.ResumeOpeningRecords.Include(x => x.Opening).ThenInclude(x=>x.Tags)
                .Include(x => x.Opening).ThenInclude(x => x.TitleClasses)
                .Where(x => x.Resume.CandidateId == rv.CandidateId &&
                        (x.CompanyName.Contains(rv.CompanyName) ||
                        x.ApplyDate.ToString().Contains(rv.ApplyDate) ||
                        x.OpeningTitle.Contains(rv.OpeningTitle) ||
                        x.Resume.Title.Contains(rv.Title)))
                .Select(p => new RecordOutputmodel
                {
                    ResumeOpeningRecordID = p.ResumeOpeningRecordId, 
                    ResumeId = (int)p.ResumeId,
                    OpeningId = (int)p.OpeningId,
                    ApplyDate = p.ApplyDate,
                    CompanyName = p.CompanyName,
                    OpeningTitle = p.OpeningTitle,
                    Title = p.Resume.Title,
                    Address = p.Opening.Address,
                    ContactName = p.Opening.ContactName,
                    ContactPhone = p.Opening.ContactPhone,
                    ContactEmail = p.Opening.ContactEmail,
                    SalaryMax = p.Opening.SalaryMax,
                    SalaryMin = p.Opening.SalaryMin,
                    Time = p.Opening.Time,
                    Description = p.Opening.Description,
                    TitleClassId = p.Opening.TitleClasses.Select(tc=>tc.TitleClassId).ToList(),
                    TagId = p.Opening.Tags.Select(t => t.TagId).ToList(),
                    Benefits = p.Opening.Benefits,
                    Degree = p.Opening.Degree,
                    CandidateId = p.Resume.CandidateId
                });

            if (query == null)
            {
                return new List<RecordOutputmodel>();
            }

            return  await query.ToListAsync();

        }
        //GET:Candidates/Record/TitleClassJson
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
        //GET:Candidates/Record/TitleCategoryJosn
        [HttpGet]
        public JsonResult TitleCategoryJson()
        {
            return Json(_context.TitleCategories.Select(tc => new
            {
                TitleCategoryId = tc.TitleCategoryId,
                TitleCategoryName = tc.TitleCategoryName
            }));
        }
        //GET:Candidates/Record/TagsJson
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
        //GET:Candidates/Record/TagClassJson
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
