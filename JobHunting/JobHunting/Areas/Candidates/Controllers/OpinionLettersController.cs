using JobHunting.Areas.Candidates.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Area("Candidates")]
    public class OpinionLettersController : Controller
    {
        private readonly DuckCandidatesContext _context;

        public OpinionLettersController(DuckCandidatesContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        //GET:Candidates/OpinionLetters/IndexJson_opinionletter
        [HttpGet]
        public JsonResult IndexJson_opinionletter()
        {
            var opinionletter = _context.OpinionLetters.Where(c => c.CandidateId != null)
                .OrderByDescending(c=>c.SendTime)
                .Select(c => new 
                {
                    LetterId= c.LetterId,
                    Class= c.Class,
                    SubjectLine= c.SubjectLine,
                    Status= c.Status,
                    Content= c.Content,
                    SendTime= c.SendTime,
                });
            return Json(opinionletter); 
        }


        //Post:Candidates/OpinionLetters/Filter
        [HttpPost]
        public async Task<IEnumerable<OpinionLetter>> Filter([FromBody] OpinionLetter opinionLetter) 
        {
            return _context.OpinionLetters.Where(
                c => c.CandidateId != null && c.Class.Contains(opinionLetter.Class) ||
                c.CandidateId != null && c.SubjectLine.Contains(opinionLetter.SubjectLine))
                .OrderByDescending(c => c.SendTime).Select(c => new OpinionLetter 
                {
                    LetterId= c.LetterId,
                    Class= c.Class,
                    SubjectLine= c.SubjectLine,                                     
                    SendTime= c.SendTime,
                    Status = c.Status,
                });
        }


        //Post:Candidates/OpinionLetters/HideLetter/{letterId}
        [HttpPost]
        public async Task<IActionResult> HideLetter([FromBody] int letterId) 
        {
            var letter = await _context.OpinionLetters.FindAsync(letterId);
            if (letter != null) { 
                letter.CandidateId = null;
            }
            try {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "隱藏失敗");
            }
            return Ok("隱藏成功");
        }


    }
}
