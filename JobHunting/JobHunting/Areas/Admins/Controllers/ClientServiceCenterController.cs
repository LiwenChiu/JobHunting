using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Admins.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace JobHunting.Areas.Admins.Controllers
{
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

        // GET: ClientServiceCenterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientServiceCenterController/Create
        public ActionResult Create()
        {
            return View();
        }

        //GET:Admins/ClientServiceCenter/IndexJson_opinionletter
        [HttpGet]
        public JsonResult IndexJson_opinionletter()
        {
            var opinionletter = _context.OpinionLetters.Select(p => new
            {
                id = p.LetterId,
                Class = p.Class,
                SubjectLine = p.SubjectLine,
                Status = p.Status,
                Content = p.Content,
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
            };

            return olovm;
        }

        //Post: Admins/ClientServiceCenter/Filter
        [HttpPost]
        public async Task<IEnumerable<OpinioLetterCardViewModel>> Filter([FromBody] OpinioLetterCardViewModel olcvm) 
        {
            return _context.OpinionLetters
                .Where(olfilter =>
                olfilter.Class.Contains(olcvm.Class) ||
                olfilter.SubjectLine.Contains(olcvm.SubjectLine) ||
                olfilter.Status==olcvm.Status)
                .Select(p => new OpinioLetterCardViewModel
                {
                    Class = p.Class,
                    SubjectLine = p.SubjectLine,
                    Status = p.Status,
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



        // POST: ClientServiceCenterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientServiceCenterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClientServiceCenterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientServiceCenterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClientServiceCenterController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
