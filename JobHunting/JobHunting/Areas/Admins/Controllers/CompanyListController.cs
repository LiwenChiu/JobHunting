using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Admins.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class CompanyListController : Controller
    {
        private readonly DuckAdminsContext _context;

        public CompanyListController(DuckAdminsContext context)
        {
            _context = context;
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
                CompanyID = p.CompanyId,
                CompanyName = p.CompanyName,
                GUINumber = p.GUINumber,
                ContactName = p.ContactName,
                ContactPhone = p.ContactPhone,
                ContactEmail = p.ContactEmail
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

        public IActionResult Index()
        {
            return View();
        }
    }
}
