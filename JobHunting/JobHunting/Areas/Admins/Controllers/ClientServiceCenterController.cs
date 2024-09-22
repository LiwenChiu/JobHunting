using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Admins.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace JobHunting.Areas.Admins.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminScheme", Roles = "admin")]
    [Area("Admins")]
    public class ClientServiceCenterController : Controller
    {
        private readonly DuckAdminsContext _context;

        public ClientServiceCenterController(DuckAdminsContext context)
        {
            _context = context;
        }
        // GET: ClientServiceCenterController
        public ActionResult Index()
        {
            return View();
        }



        //GET:Admins/ClientServiceCenter/IndexJson_opinionletter
        [HttpGet]
        public JsonResult IndexJson_opinionletter()
        {
            var opinionletter = _context.OpinionLetters.OrderByDescending(p => p.SendTime).Select(p => new
            {
                LetterId = p.LetterId,
                Class = p.Class,
                SubjectLine = p.SubjectLine,
                Status = p.Status,
                Content = p.Content,
                SendTime = p.SendTime.ToString("yyyy/MM/dd HH:mm:ss"),
            });
            return Json(opinionletter);
        }


        //Get:Admins/ClientServiceCenter/OpinionLetterModalShow/{id}
        [HttpGet]
        public async Task<OpinioLetterOutputViewModel> OpinionLetterModalShow([FromRoute] int id)
        {
            var opinionLetter = await _context.OpinionLetters.FindAsync(id);

            OpinioLetterOutputViewModel olovm = new OpinioLetterOutputViewModel
            {
                LetterId = opinionLetter.LetterId,
                Class = opinionLetter.Class,
                SubjectLine = opinionLetter.SubjectLine,
                Attachment = opinionLetter.Attachment,
                Content = opinionLetter.Content,
                Status = opinionLetter.Status,
                SendTime = opinionLetter.SendTime.ToString("yyyy/MM/dd HH:mm:ss"),
            };

            return olovm;
        }


        //Post: Admins/ClientServiceCenter/Filter
        [HttpPost]
        public async Task<IEnumerable<OpinionLetterFilterOutputViewModel>> Filter([FromBody] OpinionLetter opinionLetter)
        {
            return _context.OpinionLetters.Where(
                o =>o.Class.Contains(opinionLetter.Class) ||
                o.SubjectLine.Contains(opinionLetter.SubjectLine))
                .OrderByDescending(p => p.SendTime).Select(o => new OpinionLetterFilterOutputViewModel
                {
                    LetterId = o.LetterId,
                    Class = o.Class,
                    SubjectLine = o.SubjectLine,
                    SendTime = o.SendTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    Status = o.Status,
                });

        }


        //Post: Admins/ClientServiceCenter/ToggleStatus
        [HttpPost]
        public async Task<IActionResult> ToggleStatus([FromBody] int letterId)
        {
            var opinionLetter = await _context.OpinionLetters.FindAsync(letterId);
            if (opinionLetter == null)
            {
                return NotFound("NotFound");
            }

            opinionLetter.Status = true;
            _context.Entry(opinionLetter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return StatusCode(500, "錯誤");
            }

            return Ok("修改成功");

        }


        //GET:Admins/ClientServiceCenter/GetPicture/{id}
        [HttpGet]
        public async Task<FileResult> GetPicture(int id) 
        {
            string Filename = Path.Combine("wwwroot", "images", "No_Image_Available.jpg");
            OpinionLetter ol = await _context.OpinionLetters.FindAsync(id);
            byte[] ImageContent = ol?.Attachment!=null? ol.Attachment:System.IO.File.ReadAllBytes(Filename);
            return File(ImageContent,"image/jpg");            
        }



        //Post: Admins/ClientServiceCenter/DeleteLetter/{letterId}
        [HttpPost]
        public async Task<IActionResult> DeleteLetter([FromBody]int letterId)
        {
            var opinionLetter = await _context.OpinionLetters.FindAsync(letterId);
            if (opinionLetter != null)
            {
                _context.OpinionLetters.Remove(opinionLetter);
            }
            try
            {              
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex) 
            {
                return StatusCode(500, "刪除失敗");
            }
            return Ok("刪除成功");
        }







    }
}
