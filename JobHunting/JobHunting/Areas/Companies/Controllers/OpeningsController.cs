using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobHunting.Models;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class OpeningsController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public OpeningsController(DuckCompaniesContext context)
        {
            _context = context;
        }
        public IActionResult Opening()
        {
            return View();
        }
        //POST:Companies/Openings/filterOpening
        [HttpPost]
        public async Task<IEnumerable<OpeningsOutputModel>> filter([FromBody] OpeningsInputModel opening)
        {
            return _context.Openings.Where
                (o =>
                    o.OpeningId == opening.OpeningId ||
                    o.Title.Contains(opening.Title) ||
                    o.Address.Contains(opening.Address) ||
                    o.ContactName.Contains(opening.ContactName) ||
                    o.ContactEmail.Contains(opening.ContactEmail) ||
                    o.ContactPhone.Contains(opening.ContactPhone) ||
                    o.SalaryMax.ToString().Contains(opening.SalaryMax.ToString()) ||
                    o.SalaryMin.ToString().Contains(opening.SalaryMin.ToString()) ||
                    o.Time.Contains(opening.Time) ||
                    o.Description.Contains(opening.Description) ||
                    o.TitleClasses.Select(tc => tc.TitleClassName) == opening.TitleClasses.Select(tc => tc.TitleClassName) ||
                    o.Benefits.Contains(opening.Benefits) ||
                    o.Company.CompanyName.Contains(opening.Company.CompanyName)
                )
                .Select(c => new OpeningsOutputModel
                {
                    OpeningId = c.OpeningId,
                    Title = c.Title,
                    Address = c.Address,
                    ContactName = c.ContactName,
                    ContactEmail =c.ContactEmail,
                    ContactPhone = c.ContactPhone,
                    SalaryMax = c.SalaryMax,
                    SalaryMin = c.SalaryMin,
                    Time = c.Time,
                    Description = c.Description,
                    Benefits =c.Benefits,
                    TitleClassName = c.TitleClasses.Select(tc=>tc.TitleClassName).ToString(),
                    CompanyName = c.Company.CompanyName
                });
        }
        //GET:Companies/Home/OpeningJson
        [HttpGet]
        public JsonResult OpeningJson()
        {
            return Json(_context.Openings.Select(o => new
            {
                OpeningId = o.OpeningId,
                Title = o.Title,
                CompanyName = o.Company.CompanyName,
                Address = o.Address,
                ContactName = o.ContactName,
                ContactNumber = o.ContactPhone,
                ContactEmail = o.ContactEmail,
                SalaryMax = o.SalaryMax,
                SalaryMin = o.SalaryMin,
                Time = o.Time,
                Description = o.Description,
                TitleClassName = o.TitleClasses.Select(tc => tc.TitleClassName),
                Benefits = o.Benefits
            }));
        }
        //GET:Companies/Home/TitleClassJson
        [HttpGet]
        public JsonResult TitleClassJson()
        {
            return Json(_context.TitleClasses.Select(tc => new
            {
                TitleClassId = tc.TitleClassId,
                TitleclassName = tc.TitleClassName
            }));
        }
        //GET:Compaines/Home/TagsJson
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
