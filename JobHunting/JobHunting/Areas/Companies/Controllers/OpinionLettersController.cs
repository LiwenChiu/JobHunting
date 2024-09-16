using JobHunting.Areas.Companies.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class OpinionLettersController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public OpinionLettersController(DuckCompaniesContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        //GET:Companies/OpinionLetters/IndexJson_opinionletter
        [HttpGet]
        public JsonResult IndexJson_opinionletter() 
        {
            var opinionletter = _context.OpinionLetters.Where(p => p.CompanyId != null).OrderByDescending(p=>p.SendTime).Select(p => new 
            {
                LetterId= p.LetterId,
                Class = p.Class,
                SubjectLine = p.SubjectLine,
                Status = p.Status,
                Content = p.Content,
                SendTime = p.SendTime,

            });
            return Json(opinionletter);
        }


        //Post:Companies/OpinionLetters/Filter
        [HttpPost]
        public async Task<IEnumerable<OpinionLetter>> Filter([FromBody] OpinionLetter opinionLetter) 
        {
            return _context.OpinionLetters.Where(
                o => o.CompanyId != null && o.Class.Contains(opinionLetter.Class) ||
                o.CompanyId != null && o.SubjectLine.Contains(opinionLetter.SubjectLine))
                .OrderByDescending(p => p.SendTime).Select(o => new OpinionLetter
                {
                    LetterId = o.LetterId,
                    Class = o.Class,
                    SubjectLine = o.SubjectLine,
                    SendTime = o.SendTime,
                    Status = o.Status,
                });
        }

    }
}
