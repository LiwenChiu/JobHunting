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
    public class PricingPlanManagementController : Controller
    {
        private readonly DuckAdminsContext _context;

        public PricingPlanManagementController(DuckAdminsContext context)
        {
            _context = context;
        }

        // GET: Admins/PricingPlanManagement
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Admins/PricingPlanManagement/IndexPricingPlans
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

        // POST: Admins/PricingPlanManagement/BootFilterPage
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<PricingPlanManagementFilterOutputViewModel>> BootFilterPage([FromBody][Bind("PlanId,Title,Intro,Duration,Price,Discount,Status")] PricingPlanManagementFilterViewModel ppfvm)
       {
            return _context.PricingPlans.Select(pp => new PricingPlanManagementFilterViewModel
            {
                PlanId = pp.PlanId,
                Title = pp.Title,
                Intro = pp.Intro,
                Duration = pp.Duration,
                Price = pp.Price,
                Discount = pp.Discount,
                Status = pp.Status,
            }).Where(ppfvmfilter => ppfvmfilter.Title.Contains(ppfvm.Title) ||
                                    ppfvmfilter.Intro.Contains(ppfvm.Intro) ||
                                    ppfvmfilter.Duration.ToString().Contains(ppfvm.Duration.ToString()) ||
                                    ppfvmfilter.Price.ToString().Contains(ppfvm.Price.ToString()) ||
                                    ppfvmfilter.Discount.ToString().Contains(ppfvm.Discount.ToString()))
            .Select(ppfvmfilter => new PricingPlanManagementFilterOutputViewModel
            {
                PlanId = ppfvmfilter.PlanId,
                Title = ppfvmfilter.Title,
                Intro = ppfvmfilter.Intro,
                Duration = ppfvmfilter.Duration,
                Price = ppfvmfilter.Price,
                Discount = ppfvmfilter.Discount,
                Status = ppfvmfilter.Status,
                Edit = false,
            });
        }

        // POST: Admins/PricingPlanManagement/EditPricingPlans
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> EditPricingPlans([FromBody][Bind("PlanId,Title,Duration,Price,Discount,Status")] PricingPlanManagementEditViewModel ppevm)
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

        // POST: Admins/PricingPlanManagement/EditIntro
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> EditIntro([FromBody][Bind("PlanId,Intro")] PricingPlanManagementIntroViewModel ppivm)
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

        // POST: Admins/PricingPlanManagement/PlanStatusFalse
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

            returnStatus = [$"停用方案:{plan.Title}成功", "成功"];
            return returnStatus;
        }

        // POST: Admins/PricingPlanManagement/PlanStatusTrue
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

            returnStatus = [$"啟用方案:{plan.Title}成功", "成功"];
            return returnStatus;
        }


        // POST: Admins/PricingPlanManagement/CreatePlan
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> CreatePlan([FromBody][Bind("Title,Intro,Duration,Price,Discount")] PricingPlanManagementCreateViewModel ppcvm)
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

        // POST: Admins/PricingPlanManagement/DeletePlan
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
