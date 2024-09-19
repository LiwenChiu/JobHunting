using JobHunting.Areas.Candidates.ViewModels;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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


        //Post:Companies/OpinionLetters/IndexJson_opinionletter/{id}
        [HttpPost]
        public JsonResult IndexJson_opinionletter() 
        {
            var companyIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId))
            {
                return Json(new { error = "無法抓取 CompanyId 或使用者未登入" });
            }
            var opinionletter = _context.OpinionLetters.Where(p => p.CompanyId == companyId).OrderByDescending(p=>p.SendTime).Select(p => new 
            {
                CompanyId = p.CompanyId,
                LetterId = p.LetterId,
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
        public async Task<IEnumerable<OpinionLetterOutputModel>> Filter([FromBody] OpinionLetterInputModel opinionLetter)        
        {
            var companyIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId))
            {
                return  Enumerable.Empty<OpinionLetterOutputModel>();
            }

            return _context.OpinionLetters.Where(o=>o.CompanyId== companyId).Where(
                o => o.Class.Contains(opinionLetter.Class) ||
                o.SubjectLine.Contains(opinionLetter.SubjectLine))
                .OrderByDescending(p => p.SendTime).Select(o => new OpinionLetterOutputModel
                {   
                    CompanyId = o.CompanyId,
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
            var companyIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId))
            {
                return BadRequest("無法抓取 CompanyId 或使用者未登入");
            }
            OpinionLetter opinionLetter = new OpinionLetter();
            opinionLetter.CompanyId = companyId;
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
