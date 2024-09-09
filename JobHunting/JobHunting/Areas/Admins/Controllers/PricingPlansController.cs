using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Admins.ViewModels;

namespace JobHunting.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class PricingPlansController : Controller
    {
        private readonly DuckAdminsContext _context;

        public PricingPlansController(DuckAdminsContext context)
        {
            _context = context;
        }

        // GET: Admins/PricingPlans
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Admins/PricingPlans/IndexPricingPlans
        public JsonResult IndexPricingPlans()
        {
            return Json(_context.PricingPlans.Select(pp => new
            {
                planId = pp.PlanId,
                title = pp.Title,
                intro = pp.Intro,
                duration = pp.Duration,
                price = pp.Price,
                discount = pp.Discount,
                status = pp.Status,
                edit = false,
            }));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult BootPage([FromBody][Bind("PlanId,Title,Intro,Duration,Price,Discount,Status")] PricingPlanFilterViewModel ppfvm)
        {
            return Json(_context.PricingPlans.Select(pp => new
            {
                planId = pp.PlanId,
                title = pp.Title,
                intro = pp.Intro,
                duration = pp.Duration,
                price = pp.Price,
                discount = pp.Discount,
                status = pp.Status,
                edit = false,
            }).Where(ppfvmfilter => ppfvmfilter.planId == ppfvm.PlanId ||
                                    ppfvmfilter.title.Contains(ppfvm.Title) ||
                                    ppfvmfilter.intro.Contains(ppfvm.Intro) ||
                                    ppfvmfilter.duration == ppfvm.Duration ||
                                    ppfvmfilter.price == ppfvm.Price ||
                                    ppfvmfilter.status == ppfvm.Status));
        }

        // POST: Admins/PricingPlans/EditPricingPlans
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> EditPricingPlans([FromBody][Bind("PlanId,Title,Duration,Price,Discount,Status")] PricingPlanEditViewModel ppevm)
        {
            string[] returnStatus = new string[2];
            
            if (!ModelState.IsValid)
            {
                returnStatus = ["修改方案失敗", "失敗"];
                return returnStatus;
            }

            var pricingPlan = await _context.PricingPlans.FindAsync(ppevm.PlanId);
            pricingPlan.Title = ppevm.Title;
            pricingPlan.Duration = ppevm.Duration;
            pricingPlan.Price = ppevm.Price;
            pricingPlan.Discount = ppevm.Discount;
            pricingPlan.Status = ppevm.Status;

            _context.Entry(pricingPlan).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                returnStatus = ["修改方案失敗", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"修改方案ID:{pricingPlan.PlanId}成功", "成功"];
            return returnStatus;
        }

        // POST: Admins/PricingPlans/EditIntro
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> EditIntro([FromBody][Bind("PlanId,Intro")] PricingPlanIntroViewModel ppivm)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = ["修改方案介紹失敗", "失敗"];
                return returnStatus;
            }

            if (ppivm.Intro == null)
            {
                returnStatus = ["方案介紹不可為空", "失敗"];
                return returnStatus;
            }

            var pricingPlan = await _context.PricingPlans.FindAsync(ppivm.PlanId);
            pricingPlan.Intro = ppivm.Intro;

            _context.Entry(pricingPlan).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                returnStatus = ["修改方案介紹失敗", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"修改方案介紹ID:{pricingPlan.PlanId}成功", "成功"];
            return returnStatus;
        }

        // POST: Admins/PricingPlans/PlanStatusFalse
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> PlanStatusFalse([FromBody]int planID)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = ["停用方案失敗", "失敗"];
                return returnStatus;
            }

            var plan = await _context.PricingPlans.FindAsync(planID);
            if(plan == null)
            {
                returnStatus = ["資料庫不存在此方案", "失敗"];
                return returnStatus;
            }
            plan.Status = false;

            _context.Entry(plan).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                returnStatus = ["停用方案失敗", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"停用方案ID:{planID}成功", "成功"];
            return returnStatus;
        }

        // POST: Admins/PricingPlans/PlanStatusTrue
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> PlanStatusTrue([FromBody] int planID)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = ["啟用方案失敗", "失敗"];
                return returnStatus;
            }

            var plan = await _context.PricingPlans.FindAsync(planID);
            if (plan == null)
            {
                returnStatus = ["資料庫不存在此方案", "失敗"];
                return returnStatus;
            }
            plan.Status = true;

            _context.Entry(plan).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                returnStatus = ["啟用方案失敗", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"啟用方案ID:{planID}成功", "成功"];
            return returnStatus;
        }


        // POST: Admins/PricingPlans/createPlan
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> CreatePlan([FromBody][Bind("Title,Intro,Duration,Price,Discount")] PricingPlanCreateViewModel ppcvm)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = ["新增方案失敗", "失敗"];
                return returnStatus;
            }

            PricingPlan ppadd = new PricingPlan
            {
                Title = ppcvm.Title,
                Intro = ppcvm.Intro,
                Duration = ppcvm.Duration,
                Price = ppcvm.Price,
                Discount = ppcvm.Discount == 0 ? 1 : ppcvm.Discount,
                Status = true,
            };

            returnStatus = [$"新增方案{ppadd.Title}成功", "成功"];
            _context.PricingPlans.Add(ppadd);
            await _context.SaveChangesAsync();
            return returnStatus;
        }

        // POST: Admins/PricingPlans/DeletePlan
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> DeletePlan([FromBody]int planId)
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = ["刪除方案失敗", "失敗"];
                return returnStatus;
            }

            var plan = await _context.PricingPlans.FindAsync(planId);
            if (plan != null)
            {
                _context.PricingPlans.Remove(plan);
            }
            else
            {
                returnStatus = [$"不存在此方案ID:{planId}", "失敗"];
                return returnStatus;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                returnStatus = [$"刪除ID為{planId}的關聯方案失敗!", "失敗"];
                return returnStatus;
            }

            returnStatus = [$"刪除方案{plan.Title}成功", "成功"];
            return returnStatus;

        }
    }
}
