using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Admins.ViewModels;
using JobHunting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Admins.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminScheme", Roles = "admin")]
    [Area("Admins")]
    public class CompanyListController : Controller
    {
        private readonly DuckAdminsContext _context;
        private readonly ReviewMaillService _reviewMaillService;

        public CompanyListController(DuckAdminsContext context , ReviewMaillService reviewMaillService)
        {
            _context = context;
            _reviewMaillService = reviewMaillService;
        }


        public IActionResult CompanyList()
        {
            return View();
        }
        //GET:Admins/CompanyList/IndexJson
        [HttpGet]
        public JsonResult IndexJson()
        {
            var companyList = _context.Companies.Select(p => new
            {
                CompanyId = p.CompanyId,
                CompanyName = p.CompanyName,
                GUINumber = p.GUINumber,
                ContactName = p.ContactName,
                ContactPhone = p.ContactPhone,
                ContactEmail = p.ContactEmail,
                Status = p.Status,
                Date = p.Date,
            });
            return Json(companyList);
        }




        //Post: Admins/CompanyList/Filter
        [HttpPost]
        public async Task<IEnumerable<CompanyListViewModel>> Filter([FromBody] CompanyListViewModel clvm)
        {
            return _context.Companies
            .Where(clfilter =>
                clfilter.CompanyId == clvm.CompanyId ||
                clfilter.CompanyName.Contains(clvm.CompanyName) ||
                clfilter.GUINumber.Contains(clvm.GUINumber) ||
                clfilter.ContactName.Contains(clvm.ContactName) ||
                clfilter.ContactPhone.Contains(clvm.ContactPhone) ||
                clfilter.ContactEmail.Contains(clvm.ContactEmail))
            .Select(p => new CompanyListViewModel
            {
                CompanyId = p.CompanyId,
                CompanyName = p.CompanyName,
                GUINumber = p.GUINumber,
                ContactName = p.ContactName,
                ContactPhone = p.ContactPhone,
                ContactEmail = p.ContactEmail,
            });
        }


        [HttpPut]
        public async Task<IActionResult>EditstatusYN([FromBody] EditStatusViewmodel ESV)
        {
            var ey = await _context.Companies.FindAsync(ESV.CompanyId);

            if(ey == null)
            {
                return NotFound(new { success = false,Message = "選擇公司異常" });
            }
            ey.Status = ESV.Status;
            ey.Date = ESV.Date;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "更改狀態失敗" });
            }

            if(ey.Status == true)
            {
                _reviewMaillService.SendEmail(ESV.ContactEmail, "恭喜！您的申請已通過審核",ESV.ContactName, ESV.CompanyName);
                return Json(new { success = true, message = "更改審核狀態成功" });
            }
            else
            {
                return Json(new { success = false, message = "更改狀態失敗" });
            }
            
        }


        [HttpGet]
        public IActionResult VerifyEmail()
        {

            // 跳轉到登入頁面
            return RedirectToAction("Login", "Home");
        }




        public IActionResult Index()
        {
            return View();
        }
    }
}
