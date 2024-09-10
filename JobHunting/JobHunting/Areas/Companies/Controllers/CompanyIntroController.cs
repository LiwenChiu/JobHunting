using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class CompanyIntroController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public CompanyIntroController(DuckCompaniesContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<JsonResult> CompanyIntroList(int id)
        {
            return Json(_context.Companies.Include(a => a.CompanyClass).Where(c => c.CompanyId == id).Select(b => new
            {
                CompanyId = b.CompanyId,
                CompanyName = b.CompanyName,
                CompanyClassId = b.CompanyClassId,
                Address = b.Address,
                Intro = b.Intro,
                Benefits = b.Benefits,
                //Picture = null,
                ContactName = b.ContactName,
                ContactPhone = b.ContactPhone,
                ContactEmail = b.ContactEmail,
                CompanyClassName = b.CompanyClass.CompanyClassName
            }));
        }
        public async Task<FileResult> GetPicture(int id)
        {
            //string FileName = Path.Combine( "images", "No_Image_Available.jpg");
            Company c = await _context.Companies.FindAsync(id);
            byte[] ImageContent = c.Picture;
            return File(ImageContent, "image/jpeg");
        }
    }
}
