using Microsoft.AspNetCore.Mvc;
using JobHunting.Areas.Companies.ViewModel;
using JobHunting.Models;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Candidates.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.AspNetCore.Authorization;
namespace JobHunting.Areas.Companies.Controllers
{
    //[Authorize(Roles = "company")]
    [Area("Companies")]
    public class HomeController : Controller
    {
        private readonly DuckCompaniesContext _context; IWebHostEnvironment _hostingEnvironment;

        public HomeController(DuckCompaniesContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task<CompanyMemberDataOutputModel> GetCompanyMemberData([FromBody] int id)
        {
            return _context.Companies.AsNoTracking()
                .Where(cmd => cmd.CompanyId == id)
                .Select(cmd => new CompanyMemberDataOutputModel
                {
                    CompanyId = cmd.CompanyId,
                    CompanyName = cmd.CompanyName,
                    Intro = cmd.Intro,
                    GUINumber = cmd.GUINumber,
                    Address = cmd.Address,
                    ContactName = cmd.ContactName,
                    ContactPhone = cmd.ContactPhone,
                    ContactEmail = cmd.ContactEmail
                }).Single();
        }

        [HttpPost]
        public async Task<IEnumerable<CompanyOpeningsOutputModel>> GetCompanyOpenings([FromBody] int id)
        {
            return _context.Openings.AsNoTracking()
                .Where(o => o.CompanyId == id)
                .Select(o => new CompanyOpeningsOutputModel
                {
                    OpeningId = o.OpeningId,
                    Title = o.Title
                }).Take(4);
        }

        [HttpPost]
        public async Task<IEnumerable<CompanyResumeLikeRecordsOutputModel>> GetCompanyResumeLikeRecords([FromBody] int id)
        {
            var query = await _context.Companies.AsNoTracking()
                .Include(c => c.Resumes)
                .FirstOrDefaultAsync(c => c.CompanyId == id);

            if(query == null)
            {
                return new List<CompanyResumeLikeRecordsOutputModel>();
            }

            return query.Resumes.Select(crlro => new CompanyResumeLikeRecordsOutputModel
            {
                ResumeId = crlro.ResumeId,
                Title = crlro.Title
            }).Take(4);
        }

        public async Task<FileResult> GetPicture(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string FileName = Path.Combine(webRootPath, "images", "No_Image_Available.jpg");
            Models.Company c = await _context.Companies.FindAsync(id);
            byte[] ImageContent = c?.Picture != null ? c.Picture : System.IO.File.ReadAllBytes(FileName);
            return File(ImageContent, "image/jpeg");
        }
        /*------------------------------------------*/

        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Member()
        {
            return View();
        }
        public IActionResult Intro()
        {
            return View();
        }

        public IActionResult Resume()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Message()
        {
            return View();
        }

        public IActionResult OpinionLetters()
        {
            return View();
        }
        public IActionResult PricingPlans()
        {
            return View();
        }
        public IActionResult PricingOrderHistory()
        {
            return View();
        }

    }
}
