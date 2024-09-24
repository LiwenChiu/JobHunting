using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Admins.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Admins.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminScheme", Roles = "admin")]
    [Area("Admins")]
    public class CandidateListController : Controller
    {
        private readonly DuckAdminsContext _context;

        public CandidateListController(DuckAdminsContext context)
        {
            _context = context;
        }
        public IActionResult CandidateList()
        {
            return View();
        }
        //GET:Admins/CandidateList/IndexJson_candidate
        [HttpGet]
        public JsonResult IndexJson_candidate()
        {
            var candidateList = _context.Candidates.Select(p => new
            {
                CandidateId = p.CandidateId,
                Name = p.Name,
                Sex = p.Sex,
                Degree = p.Degree,
                Address = p.Address,
                VerifyEmailYN = p.VerifyEmailYN,
                RegisterTime = p.RegisterTime,
                Email= p.Email,
            });
            return Json(candidateList);
        }


        //Post: Admins/CandidateList/Filter
        [HttpPost]
        public async Task<IEnumerable<CandidateListViewModel>> Filter([FromBody] CandidateListViewModel cdvm)
        {
            return _context.Candidates
            .Where(cdfilter =>
                cdfilter.CandidateId == cdvm.CandidateId ||
                cdfilter.Name.Contains(cdvm.Name) ||
                cdfilter.Sex == cdvm.Sex ||
                cdfilter.Degree.Contains(cdvm.Degree) ||
                cdfilter.Address.Contains(cdvm.Address))
            .Select(p => new CandidateListViewModel
            {
                CandidateId = p.CandidateId,
                Sex = p.Sex,
                Name = p.Name,
                Degree = p.Degree,
                Address = p.Address,
                VerifyEmailYN = p.VerifyEmailYN,
                RegisterTime = p.RegisterTime,
                Email = p.Email,
            });
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
