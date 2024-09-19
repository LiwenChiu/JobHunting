using JobHunting.Areas.Candidates.ViewModels;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
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

        //GET:Companies/OpinionLetters/OpinionLetterModalShow/{id}
        [HttpGet]
        public async Task<Com_OpinionLetterModalViewModel> OpinionLetterModalShow([FromRoute] int id) 
        {
            var opinionLetter = await _context.OpinionLetters.FindAsync(id);
            Com_OpinionLetterModalViewModel comolmv = new Com_OpinionLetterModalViewModel
            {
                LetterId = opinionLetter.LetterId,
                Class = opinionLetter.Class,
                SubjectLine = opinionLetter.SubjectLine,
                Attachment = opinionLetter.Attachment,
                Content = opinionLetter.Content,
                Status = opinionLetter.Status,
                SendTime = opinionLetter.SendTime,
            };
            return comolmv;
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

        //Post:Companies/OpinionLetters/HideLetter/{letterId}
        [HttpPost]
        public async Task<IActionResult> HideLetter([FromBody]int letterId) 
        {
            var letter = await _context.OpinionLetters.FindAsync(letterId);
            if (letter != null) {
                letter.CompanyId = null;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                return StatusCode(500, "隱藏失敗");
            }
            return Ok("隱藏成功");
        }


        //Post:Companies/OpinionLetters/AddLetter
        [HttpPost]
        public async Task<IActionResult> AddLetter([FromForm] CompanyInsertLetter letter)
        {
            OpinionLetter opinionLetter = new OpinionLetter();
            opinionLetter.CompanyId = letter.CompanyId;
            opinionLetter.Class = letter.Letterclass;
            opinionLetter.SubjectLine = letter.SubjectLine;
            opinionLetter.Content = letter.Content;
            opinionLetter.SendTime = letter.SendTime;

            if (letter.ImageFile != null)
            {
                using (BinaryReader br = new BinaryReader(letter.ImageFile.OpenReadStream()))
                {
                    opinionLetter.Attachment = br.ReadBytes((int)letter.ImageFile.Length);
                }
            }

            _context.OpinionLetters.Add(opinionLetter);
            await _context.SaveChangesAsync();

            return Ok("新增意見信件成功");
        }

    }
}
