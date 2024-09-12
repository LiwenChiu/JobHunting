using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobHunting.Models;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using System.Numerics;
using System.Net;
namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class OpeningsController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public OpeningsController(DuckCompaniesContext context)
        {
            _context = context;
        }
        public IActionResult Opening()
        {
            return View();
        }

        //GET:Companies/Openings/OpeningJson
        [HttpGet]
        public JsonResult OpeningJson()
        {
            return Json(_context.Openings.Select(o => new
            {
                OpeningId = o.OpeningId,
                Title = o.Title,
                CompanyName = o.Company.CompanyName,
                Address = o.Address,
                ContactName = o.ContactName,
                ContactPhone = o.ContactPhone,
                ContactEmail = o.ContactEmail,
                SalaryMax = o.SalaryMax,
                SalaryMin = o.SalaryMin,
                Time = o.Time,
                Description = o.Description,
                TitleClassId=o.TitleClasses.Select(tc=>tc.TitleClassId),
                TitleClassName = o.TitleClasses.Select(tc => tc.TitleClassName),
                TagId=o.Tags.Select(t=>t.TagId),
                TagName=o.Tags.Select(t=>t.TagName),
                Benefits = o.Benefits,
                Edit = false,
                Degree = o.Degree,
                ReleaseYN = o.ReleaseYN
            }));
        }

        [HttpPost]


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
        public async Task<IActionResult> EditOpening([FromBody][Bind("Title", "Address", "ContactName", "ContactPhone", "ContactEmail", "SalaryMax", "SalaryMin", "Time", "Benefits", "Description", "ReleaseYN", "Degree", "TitleClassName", "TitleClassId")] OpeningsInputModel oim)
        {
            var opening = await _context.Openings
                .Include(o => o.TitleClasses).Include(t=>t.Tags)
                .FirstOrDefaultAsync(o => o.OpeningId == oim.OpeningId);

            var titleClasses = await _context.TitleClasses
                    .Where(tc => oim.TitleClassId.Contains(tc.TitleClassId))
                    .ToListAsync();

            var tags = await _context.Tags
                .Where(tc => oim.TagId.Contains(tc.TagId))
                .ToListAsync();

            if (opening == null)
            {
                return NotFound(new { Message = "Opening not found" });
            }

            // 更新 Opening 的屬性
            opening.Title = oim.Title;
            opening.Address = oim.Address;
            opening.ContactName = oim.ContactName;
            opening.ContactPhone = oim.ContactPhone;
            opening.ContactEmail = oim.ContactEmail;
            opening.SalaryMax = oim.SalaryMax;
            opening.SalaryMin = oim.SalaryMin;
            opening.Time = oim.Time;
            opening.Benefits = oim.Benefits;
            opening.Description = oim.Description;
            opening.ReleaseYN = oim.ReleaseYN;
            opening.Degree = oim.Degree;

            opening.TitleClasses.Clear();
            opening.Tags.Clear();


            foreach (var titleClass in titleClasses)
            {
                opening.TitleClasses.Add(titleClass);
            }

            foreach (var tag in tags)
            {
                opening.Tags.Add(tag);
            }

            await _context.SaveChangesAsync();
            return Json(new { message = "修改成功" });
        }
        
        [HttpPost]
        public async Task<Array> DelOpening([FromBody] int openingId)
        {
            string[] returnStatus = new string[2];
            if (!ModelState.IsValid)
            {
                returnStatus = ["刪除職缺失敗", "失敗"];
                return returnStatus;
            }

            var opening = await _context.Openings.FindAsync(openingId);
            if (opening != null)
            {
                _context.Openings.Remove(opening);
            }
            else
            {
                returnStatus = [$"不存在此職缺ID:{openingId}", "失敗"];
                return returnStatus;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                returnStatus = [$"刪除ID為{openingId}的關聯職缺失敗!", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"刪除職缺{opening.Title}成功", "成功"];
            return returnStatus;
        }
       
        [HttpPost]
        public async Task<IActionResult> CreateOpening([FromBody] CreateOpeningInputModel coim)
        {
            try
            {
                var company = await _context.Companies.FindAsync(coim.CompanyId);
                if (company == null)
                {
                    return NotFound(new { Message = "沒有此公司" });
                }

                var titleClasses = await _context.TitleClasses
                    .Where(tc => coim.TitleClassId.Contains(tc.TitleClassId))
                    .ToListAsync();

                var tags = await _context.Tags
                    .Where(t => coim.TagId.Contains(t.TagId))
                    .ToListAsync();

                Models.Opening opening = new Models.Opening
                { 
                    Title = coim.Title,
                    Address = coim.Address,
                    ContactName = coim.ContactName,
                    ContactPhone = coim.ContactName,
                    ContactEmail = coim.ContactEmail,
                    SalaryMax = coim.SalaryMax,
                    SalaryMin = coim.SalaryMin,
                    Time = coim.Time,
                    Benefits = coim.Benefits,
                    Description = coim.Description,
                    CompanyId = coim.CompanyId,
                    ReleaseYN = coim.ReleaseYN,
                    Degree = coim.Degree,
                    
                };

                foreach (var titleClass in titleClasses)
                {
                    opening.TitleClasses.Add(titleClass);
                }

                foreach (var tag in tags)
                {
                    opening.Tags.Add(tag);
                }

                _context.Openings.Add(opening);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new { message = "新增職缺失敗" });
            };
            return Json(new { success = true, message = "新增職缺成功" });
        }
    }
}
