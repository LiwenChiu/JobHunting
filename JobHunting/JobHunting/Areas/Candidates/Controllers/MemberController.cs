using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using JobHunting.Areas.Companies.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using System.Security.Claims;

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

        //POST: Candidates/Member/GetWholeCandidateMemberData
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<GetWholeCandidateMemberDataViewModel>> GetWholeCandidateMemberData()
        {
            var CandidateId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CandidateId))
            {
                return new List<GetWholeCandidateMemberDataViewModel>(); // 或處理未授權訪問的情況
            }

            return _context.Candidates
                .Where(c => c.CandidateId.ToString() == CandidateId)
                .Select(c => new GetWholeCandidateMemberDataViewModel
                {
                    CandidateId = int.Parse(CandidateId),
                    Name = c.Name,
                    Email = c.Email,
                    Sex = c.Sex,
                    Birthday = c.Birthday,
                    Phone = c.Phone,
                    Address = c.Address,
                    Degree = c.Degree,
                    EmploymentStatus = c.EmploymentStatus,
                    MilitaryService = c.MilitaryService
                });
        }

        //POST: Candidates/Member/SaveEditMemberData
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> SaveEditMemberData([FromBody][Bind("Name,Email,Sex,Birthday,Phone,Address,Degree,EmploymentStatus,MilitaryService")] GetWholeCandidateMemberDataViewModel gwcmdvm)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = ["修改會員資料失敗", "失敗"];
                return returnStatus;
            }

            var candidateIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(candidateIdClaim) || !int.TryParse(candidateIdClaim, out int candidateId))
            {
                returnStatus = ["修改會員資料失敗", "失敗"];
                return returnStatus;
            }

            var candidatememberData = await _context.Candidates.FindAsync(candidateId);
            if (candidatememberData == null)
            {
                returnStatus = ["會員資料不存在", "失敗"];
                return returnStatus;
            }
            candidatememberData.Name = gwcmdvm.Name;
            candidatememberData.Email = gwcmdvm.Email;
            candidatememberData.Sex = gwcmdvm.Sex;
            candidatememberData.Birthday = gwcmdvm.Birthday;
            candidatememberData.Phone = gwcmdvm.Phone;
            candidatememberData.Address = gwcmdvm.Address;
            candidatememberData.Degree = gwcmdvm.Degree;
            candidatememberData.EmploymentStatus = gwcmdvm.EmploymentStatus;
            candidatememberData.MilitaryService = gwcmdvm.MilitaryService;

            _context.Entry(candidatememberData).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                returnStatus = ["修改會員資料失敗", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"修改會員資料成功", "成功"];
            return returnStatus;
        }
    }
}
