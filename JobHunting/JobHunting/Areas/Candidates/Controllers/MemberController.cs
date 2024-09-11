using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Area("Candidates")]
    public class MemberController : Controller
    {
        private readonly DuckCandidatesContext _context;

        public MemberController(DuckCandidatesContext context)
        {
            _context = context;
        }

        //GET: Candidaes/Member
        public IActionResult Index()
        {
            return View();
        }

        //POST: Candidates/Member/getWholeCandidateMemberData
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<GetWholeCandidateMemberDataViewModel>> getWholeCandidateMemberData([FromBody]int id)
        {
            return _context.Candidates
                .Where(c => c.CandidateId == id)
                .Select(c => new GetWholeCandidateMemberDataViewModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    Sex = c.Sex == true ? "男" : "女",
                    Birthday = c.Birthday,
                    TitleClassId = c.TitleClassId,
                    Phone = c.Phone,
                    Address = c.Address,
                    Degree = c.Degree,
                    EmploymentStatus = c.EmploymentStatus,
                    MilitaryService = c.MilitaryService
                });
        }
    }
}
