using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //POST: Candidates/Member/GetWholeCandidateMemberData
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<GetWholeCandidateMemberDataViewModel>> GetWholeCandidateMemberData([FromBody]int id)
        {
            return _context.Candidates
                .Where(c => c.CandidateId == id)
                .Select(c => new GetWholeCandidateMemberDataViewModel
                {
                    CandidateId = c.CandidateId,
                    Name = c.Name,
                    Email = c.Email,
                    Sex = c.Sex,
                    Birthday = c.Birthday,
                    TitleClassId = c.TitleClassId,
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
        public async Task<Array> SaveEditMemberData([FromBody][Bind("CandidateId,Name,Email,Sex,Birthday,TitleClassId,Phone,Address,Degree,EmploymentStatus,MilitaryService")] GetWholeCandidateMemberDataViewModel gwcmdvm)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = ["修改會員資料失敗", "失敗"];
                return returnStatus;
            }

            var candidatememberData = await _context.Candidates.FindAsync(gwcmdvm.CandidateId);
            if (candidatememberData == null)
            {
                returnStatus = ["會員資料不存在", "失敗"];
                return returnStatus;
            }
            candidatememberData.Name = gwcmdvm.Name;
            candidatememberData.Email = gwcmdvm.Email;
            candidatememberData.Sex = gwcmdvm.Sex;
            candidatememberData.Birthday = gwcmdvm.Birthday;
            candidatememberData.TitleClassId = gwcmdvm.TitleClassId;
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
