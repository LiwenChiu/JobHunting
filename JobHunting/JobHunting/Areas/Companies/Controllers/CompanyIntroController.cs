using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using JobHunting.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class CompanyIntroController : Controller
    {
        private readonly DuckCompaniesContext _context;
                         IWebHostEnvironment _hostingEnvironment;

        public CompanyIntroController(DuckCompaniesContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetCompanyClass()
        {
            return Json(_context.CompanyClasses.Select(a => new IntroCompanyClassSelect
            {
                CompanyClassId = a.CompanyClassId,
                CompanyClassName = a.CompanyClassName,
            }));
        }
        [HttpPost]
        public async Task<JsonResult> CompanyIntroList()
        {
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(CompanyId))
            {
                return Json(new { message = "未授權訪問" });
            }
            return Json(_context.Companies.Include(a => a.CompanyClass).Where(c => c.CompanyId.ToString() == CompanyId).Select(b => new CompanyIntroViewModel
            {
                CompanyId = int.Parse(CompanyId),
                CompanyName = b.CompanyName,
                GUINumber = b.GUINumber,
                CompanyClassId = b.CompanyClassId,
                Address = b.Address,
                Intro = b.Intro,
                Benefits = b.Benefits,
                ContactName = b.ContactName,
                ContactPhone = b.ContactPhone,
                ContactEmail = b.ContactEmail,
                CompanyClassName = b.CompanyClass.CompanyClassName,
                Edit = false,
            }));
        }
        public async Task<FileResult> GetPicture(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string FileName = Path.Combine(webRootPath,"images", "No_Image_Available.jpg");
            Company c = await _context.Companies.FindAsync(id);
            byte[] ImageContent = c?.Picture != null ? c.Picture : System.IO.File.ReadAllBytes(FileName);
            return File(ImageContent, "image/jpeg");
        }
        [HttpPost]
        public async Task<string> PutCompanyIntro([FromForm] PutCompanyIntroInput companyIntro)
        {
            var companyIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId))
            {
                return "未授權訪問";
            }
            Company co = await _context.Companies.FindAsync(companyId);
            if (co == null)
            {
                return "修改員工紀錄失敗!";
            }
            else
            {
                co.CompanyName = companyIntro.CompanyName;
                co.Address= companyIntro.Address;
                co.Intro = companyIntro.Intro;
                co.GUINumber = companyIntro.GUINumber;
                co.Benefits = companyIntro.Benefits;
                co.ContactName = companyIntro.ContactName;
                co.ContactPhone = companyIntro.ContactPhone;
                co.ContactEmail = companyIntro.ContactEmail;
                co.CompanyClassId = companyIntro.CompanyClassId;
                IsPicture(companyIntro, co);
                _context.Entry(co).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(companyId))
                    {
                        return "修改員工紀錄失敗!";
                    }
                    else
                    {
                        throw;
                    }
                }
                return "修改員工紀錄成功!";
            }
        }
        private bool CompanyExists(int companyId)
        {
            return _context.Companies.Any(e => e.CompanyId == companyId);
        }
        private static void IsPicture(PutCompanyIntroInput companyIntro, Company c)
        {
            if (companyIntro.ImageFile != null)
            {
                using (BinaryReader br = new BinaryReader(companyIntro.ImageFile.OpenReadStream()))
                {
                    c.Picture = br.ReadBytes((int)companyIntro.ImageFile.Length);
                }
            }
        }
    }
}
