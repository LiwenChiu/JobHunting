using JobHunting.Areas.Candidates.Models;
using JobHunting.Areas.Candidates.ViewModels;
using JobHunting.Areas.Companies.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobHunting.Areas.Candidates.Controllers
{
    [Authorize(Roles = "candidate")]
    [Area("Candidates")]
    public class OpeningStorageController : Controller
    {
        private readonly DuckCandidatesContext _context;

        public OpeningStorageController(DuckCandidatesContext context)
        {
            _context = context;
        }
        public IActionResult index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IEnumerable<CandidateOpeningStorageOutputModel>> CandidateOpenings([FromBody] CandidateOpeningStorageInputModel cosm)
        {
            var CandidateId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(CandidateId))
            {
                return new List<CandidateOpeningStorageOutputModel>(); // 或處理未授權訪問的情況
            }

            var query = _context.Candidates.Include(x => x.Openings).ThenInclude(x => x.Tags)
                .Include(x => x.Openings).ThenInclude(x => x.TitleClasses)
                .Include(x => x.Openings).ThenInclude(x => x.Company)
                .Where(x => x.CandidateId.ToString() == CandidateId)
                .SelectMany(x => x.Openings)
                .Where (o => 
                    o.Title.Contains(cosm.Title) ||
                    o.Address.Contains(cosm.Address) ||
                    o.Time.Contains(cosm.Time))
                .Select(ror => new CandidateOpeningStorageOutputModel
                {
                    OpeningId = ror.OpeningId,
                    Title = ror.Title,
                    CompanyName = ror.Company.CompanyName,
                    Address = ror.Address,
                    RequiredNumber = ror.RequiredNumber,
                    ResumeNumber = ror.ResumeNumber,
                    ContactName = ror.ContactName,
                    ContactPhone = ror.ContactPhone,
                    ContactEmail = ror.ContactEmail,
                    SalaryMax = ror.SalaryMax,
                    SalaryMin = ror.SalaryMin,
                    Time = ror.Time,
                    Description = ror.Description,
                    TitleClassId = ror.TitleClasses.Select(tc => tc.TitleClassId).ToList(),
                    TagId = ror.Tags.Select(t => t.TagId).ToList(),
                    Benefits = ror.Benefits,
                    Degree = ror.Degree,
                });

            if (query == null)
            {
                return new List<CandidateOpeningStorageOutputModel>();
            }
            return await query.ToListAsync();
        }




        //GET:Companies/Openings/TitleClassJson
        [HttpGet]
        public JsonResult TitleClassJson()
        {
            return Json(_context.TitleClasses.Select(tc => new
            {
                TitleClassId = tc.TitleClassId,
                TitleClassName = tc.TitleClassName,
                TitleCategoryId = tc.TitleCategoryId
            }));
        }
        //GET:Companies/Openings/TitleCategoryJosn
        [HttpGet]
        public JsonResult TitleCategoryJson()
        {
            return Json(_context.TitleCategories.Select(tc => new
            {
                TitleCategoryId = tc.TitleCategoryId,
                TitleCategoryName = tc.TitleCategoryName
            }));
        }
        //GET:Compaines/Openings/TagsJson
        [HttpGet]
        public JsonResult TagJson()
        {
            return Json(_context.Tags.Select(t => new
            {
                TagId = t.TagId,
                TagName = t.TagName,
                TagClassId = t.TagClassId
            }));
        }
        //GET:Companies/Openings/TagClassJson
        [HttpGet]
        public JsonResult TagClassJson()
        {
            return Json(_context.TagClasses.Select(tc => new
            {
                TagClassId = tc.TagClassId,
                TagClassName = tc.TagClassName
            }));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCdOpRelation([FromBody]RemoveCdOpRelationInputModel rcor)
        {
            var CandidateId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(CandidateId))
            {
                return Unauthorized(new { message = "未授權訪問" });
            }

            var candidate = await _context.Candidates
                .Include(c => c.Openings)
                .FirstOrDefaultAsync(c => c.CandidateId.ToString() == CandidateId);

            if (candidate == null)
            {
                return NotFound(new { message = "Candidate not found!" });
            }

            var opening = candidate.Openings.FirstOrDefault(o => o.OpeningId == rcor.OpeningId);

            if (opening != null)
            {
                candidate.Openings.Remove(opening); // 從關聯表中移除
                await _context.SaveChangesAsync();   // 保存變更
            }

            return Ok(new { message = "刪除收藏職缺成功!" });
        }

        public async Task<IEnumerable<OpeningStorageResumesOutputViewModel>> ResumesJson()
        {
            var candidateIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(candidateIdClaim) || !int.TryParse(candidateIdClaim, out int candidateId))
            {
                return Enumerable.Empty<OpeningStorageResumesOutputViewModel>();
            }
            var resumes = _context.Resumes.AsNoTracking()
                .Where(r => r.CandidateId == candidateId && r.ReleaseYN == true)
                .Select(r => new OpeningStorageResumesOutputViewModel
                {
                    ResumeId = r.ResumeId,
                    ResumeTitle = r.Title,
                });

            return resumes;
        }

        //POST: Candidates/OpeningStorage/ApplyJob
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<CandidatesApplyJobOutputViewModel> ApplyJob([FromBody][Bind("resumeId,openingId")] CandidatesApplyJobViewModel cajvm)
        {
            if (!ModelState.IsValid)
            {
                return new CandidatesApplyJobOutputViewModel
                { 
                    AlertText = "失敗",
                    AlertStatus = false,
                };
            }
            var candidateIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(candidateIdClaim) || !int.TryParse(candidateIdClaim, out int candidateId))
            {
                return new CandidatesApplyJobOutputViewModel();
            }

            var Candidate = await _context.Candidates.FindAsync(candidateId);
            if (Candidate == null)
            {
                return new CandidatesApplyJobOutputViewModel
                {
                    AlertText = "失敗",
                    AlertStatus = false,
                };
            }

            var resume = await _context.Resumes.FindAsync(cajvm.resumeId);
            if (resume == null)
            {
                return new CandidatesApplyJobOutputViewModel
                {
                    AlertText = "失敗",
                    AlertStatus = false,
                };
            }

            if(resume.ReleaseYN == false)
            {
                return new CandidatesApplyJobOutputViewModel
                {
                    AlertText = "履歷未開放",
                    AlertStatus = false,
                };
            }

            if (Candidate.Name == null || Candidate.Sex == null || Candidate.Birthday == null || Candidate.Phone == null || Candidate.Degree == null)
            {
                return new CandidatesApplyJobOutputViewModel
                {
                    AlertText = "會員資料不全",
                    AlertStatus = false,
                };
            }

            List<ResumeOpeningRecord> record = _context.ResumeOpeningRecords.Where(ror => ror.ResumeId == cajvm.resumeId && ror.OpeningId == cajvm.openingId).ToList();
            if(record.Count > 0)
            {
                return new CandidatesApplyJobOutputViewModel
                { 
                    AlertText = "已有應徵紀錄",
                    AlertStatus = false 
                };
            }

            var companyOpening = await _context.Openings.AsNoTracking().Include(o => o.Company).Where(o => o.OpeningId == cajvm.openingId).Select(o => new
            {
                OpeningId = o.OpeningId,
                OpeningTitle = o.Title,
                CompanyId = o.CompanyId,
                CompanyName = o.Company.CompanyName,
            }).FirstOrDefaultAsync();

            if (companyOpening == null)
            {
                return new CandidatesApplyJobOutputViewModel
                {
                    AlertText = "失敗",
                    AlertStatus = false
                };
            }

            var opening = await _context.Openings.FindAsync(cajvm.openingId);
            opening.ResumeNumber++;

            _context.Entry(opening).State = EntityState.Modified;

            ResumeOpeningRecord recordResumeOpening = new ResumeOpeningRecord
            {
                ResumeId = cajvm.resumeId,
                OpeningId = companyOpening.OpeningId,
                OpeningTitle = companyOpening.OpeningTitle,
                CompanyId = companyOpening.CompanyId,
                CompanyName = companyOpening.CompanyName,
                ApplyDate = DateOnly.FromDateTime(DateTime.Now),
                InterviewYN = false,
                HireYN = false,
            };

            _context.ResumeOpeningRecords.Add(recordResumeOpening);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                return new CandidatesApplyJobOutputViewModel
                {
                    AlertText = "失敗",
                    AlertStatus = false
                };
            }

            return new CandidatesApplyJobOutputViewModel
            {
                AlertText = $"以 {resume.Title} 應徵 {companyOpening.CompanyName} 的 {companyOpening.OpeningTitle} 成功",
                AlertStatus = true,
            };
        }
    }
}
