using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModel;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Authorize(Roles = "candidate")]
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


        //Post:Candidates/OpinionLetters/IndexJson_opinionletter/{id}
        [HttpPost]
        public JsonResult IndexJson_opinionletter()
        {
            var candidateIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(candidateIdClaim) || !int.TryParse(candidateIdClaim, out int candidateId))
            {
                return Json(new { error = "無法抓取 CompanyId 或使用者未登入" });
            }
            var opinionletter = _context.OpinionLetters.Where(c => c.CandidateId== candidateId)
                .OrderByDescending(c=>c.SendTime)
                .Select(c => new 
                {
                    CandidateId=c.CandidateId,
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
        public async Task<IEnumerable<OpinionLetterOutputModel>> Filter([FromBody] OpinionLetterInputModel opinionLetter) 
        {
            var candidateIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(candidateIdClaim) || !int.TryParse(candidateIdClaim, out int candidateId))
            {
                return Enumerable.Empty<OpinionLetterOutputModel>();
            }
            return _context.OpinionLetters.Where(c=>c.CandidateId== candidateId).Where(
                c => c.CandidateId != null && c.Class.Contains(opinionLetter.Class) ||
                c.CandidateId != null && c.SubjectLine.Contains(opinionLetter.SubjectLine))
                .OrderByDescending(c => c.SendTime).Select(c => new OpinionLetterOutputModel
                {
                    LetterId= c.LetterId,
                    Class= c.Class,
                    SubjectLine= c.SubjectLine,                                     
                    SendTime= c.SendTime,
                    Status = c.Status,
                });
        }

        //GET: Candidates/OpinionLetters/GetCandidateData
        public async Task<GetCandidateDataOutputViewModel> GetCandidateData()
        {
            var candidateIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(candidateIdClaim) || !int.TryParse(candidateIdClaim, out int candidateId))
            {
                return new GetCandidateDataOutputViewModel { AlertText = "失敗" };
            }

            var candidateData = await _context.Candidates.AsNoTracking()
                .Where(c => c.CandidateId == candidateId)
                .Select(c => new GetCandidateDataInputViewModel
                {
                    Name = c.Name,
                    Sex = c.Sex,
                    Birthday = c.Birthday,
                    Phone = c.Phone,
                    Address = c.Address,
                    Degree = c.Degree,
                    VerifyEmailYN = c.VerifyEmailYN,
                }).FirstOrDefaultAsync();

            if (candidateData == null)
            {
                return new GetCandidateDataOutputViewModel { AlertText = "失敗" };
            }

            if (!candidateData.VerifyEmailYN)
            {
                return new GetCandidateDataOutputViewModel { DataStatus = false, AlertText = "驗證信箱" };
            }

            if (string.IsNullOrEmpty(candidateData.Name) || !candidateData.Sex.HasValue || !candidateData.Birthday.HasValue || string.IsNullOrEmpty(candidateData.Phone) || string.IsNullOrEmpty(candidateData.Address) || string.IsNullOrEmpty(candidateData.Degree))
            {
                return new GetCandidateDataOutputViewModel { DataStatus = false, AlertText = "完整填寫會員資料" };
            }

            return new GetCandidateDataOutputViewModel { DataStatus = true, AlertText = "新增意見信件" };
        }

        //GET:Candidates/OpinionLetters/OpinionLetterModalShow/{id}
        [HttpGet]
        public async Task<Can_OpinionLetterModalViewModel> OpinionLetterModalShow([FromRoute] int id) 
        {
            var opinionLetter = await _context.OpinionLetters.FindAsync(id);
            Can_OpinionLetterModalViewModel canolmv = new Can_OpinionLetterModalViewModel
            {
                LetterId= opinionLetter.LetterId,
                Class= opinionLetter.Class,
                SubjectLine = opinionLetter.SubjectLine,
                Attachment = opinionLetter.Attachment,
                Content = opinionLetter.Content,
                Status = opinionLetter.Status,
                SendTime = opinionLetter.SendTime,
            };
            return canolmv;
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

        //Post:Candidates/OpinionLetters/AddLetter
        [HttpPost]
        public async Task<IActionResult> AddLetter([FromForm] CandidateInsertLetterViewModel letter) 
        {
            var candidateIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(candidateIdClaim) || !int.TryParse(candidateIdClaim, out int candidateId))
            {
                return BadRequest("無法抓取 CompanyId 或使用者未登入");
            }
            OpinionLetter opinionLetter = new OpinionLetter();
            opinionLetter.CandidateId = candidateId;
            opinionLetter.Class = letter.Letterclass;
            opinionLetter.SubjectLine = letter.SubjectLine;
            opinionLetter.Content = letter.Content;
            //opinionLetter.SendTime = letter.SendTime;
            var taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            opinionLetter.SendTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taiwanTimeZone);

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


        //GET:Candidates/OpinionLetters/GetPicture/{id}
        [HttpGet]
        public async Task<FileResult> GetPicture(int id)
        {
            string Filename = Path.Combine("wwwroot", "images", "No_Image_Available.jpg");
            OpinionLetter ol = await _context.OpinionLetters.FindAsync(id);
            byte[] ImageContent = ol?.Attachment != null ? ol.Attachment : System.IO.File.ReadAllBytes(Filename);
            return File(ImageContent, "image/jpg");
        }

    }
}
