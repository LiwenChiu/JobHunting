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
using System.Security.Claims;
namespace JobHunting.Areas.Companies.Controllers
{
    [Authorize(Roles = "company")]
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
        public async Task<CompanyMemberDataOutputModel> GetCompanyMemberData()
        {
            // 從 claims 中取得 CompanyId
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return new CompanyMemberDataOutputModel(); // 或處理未授權訪問的情況
            }

            var companyData = await _context.Companies.AsNoTracking()
        .Where(cmd => cmd.CompanyId.ToString() == CompanyId)
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
        }).SingleOrDefaultAsync();

            // 如果 companyData 為 null，返回空的 CompanyMemberDataOutputModel
            return companyData ?? new CompanyMemberDataOutputModel();
        }

        [HttpPost]
        public async Task<IEnumerable<CompanyOpeningsOutputModel>> GetCompanyOpenings()
        {
            // 從 claims 中取得 CompanyId
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return new List<CompanyOpeningsOutputModel>(); // 或處理未授權訪問的情況
            }

            // 使用 CompanyId 查詢開放職位
            return await _context.Openings.AsNoTracking()
                .Where(o => o.Company.CompanyId.ToString() == CompanyId)
                .Select(o => new CompanyOpeningsOutputModel
                {
                    OpeningId = o.OpeningId,
                    Title = o.Title
                }).Take(4).ToListAsync();
        }


        [HttpPost]
        public async Task<IEnumerable<CompanyResumeLikeRecordsOutputModel>> GetCompanyResumeLikeRecords()
        {
            // 從 claims 中取得 CompanyId
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CompanyId))
            {
                return new List<CompanyResumeLikeRecordsOutputModel>(); // 或處理未授權訪問的情況
            }

            // 使用 CompanyId 查詢公司喜好紀錄
            var company = await _context.Companies.AsNoTracking()
                .Include(c => c.Resumes)
                .FirstOrDefaultAsync(c => c.CompanyId.ToString() == CompanyId);

            if (company == null)
            {
                return new List<CompanyResumeLikeRecordsOutputModel>();
            }

            return company.Resumes.Select(crlro => new CompanyResumeLikeRecordsOutputModel
            {
                ResumeId = crlro.ResumeId,
                Title = crlro.Title
            }).Take(4).ToList();
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
        public IActionResult Intro()
        {
            return View();
        }

        public IActionResult Resume()
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
