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
                planID = pp.PlanID,
                title = pp.Title,
                intro = pp.Intro,
                duration = pp.Duration,
                price = pp.Price,
                discount = pp.Discount,
                status = pp.Status,
                edit = false,
            }));
        }

        // POST: Admins/PricingPlans/EditPricingPlans
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> EditPricingPlans([FromBody][Bind("PlanID,Title,Duration,Price,Discount,Status")] PricingPlanViewModel ppvm)
        {
            string[] returnStatus = new string[2];
            
            if (!ModelState.IsValid)
            {
                returnStatus = ["修改方案失敗", "失敗"];
                return returnStatus;
            }

            var pricingPlan = await _context.PricingPlans.FindAsync(ppvm.PlanID);
            pricingPlan.Title = ppvm.Title;
            pricingPlan.Duration = ppvm.Duration;
            pricingPlan.Price = ppvm.Price;
            pricingPlan.Discount = ppvm.Discount;
            pricingPlan.Status = ppvm.Status;

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

            returnStatus = [$"修改方案ID:{pricingPlan.PlanID}成功", "成功"];
            return returnStatus;
        }

        // POST: Admins/PricingPlans/EditIntro
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> EditIntro([FromBody][Bind("PlanID,Intro")] PricingPlanIntroViewModel ppivm)
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

            var pricingPlan = await _context.PricingPlans.FindAsync(ppivm.PlanID);
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

            returnStatus = [$"修改方案介紹ID:{pricingPlan.PlanID}成功", "成功"];
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

        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public async Task<Array> 

        //// GET: Admins/PricingPlans/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var pricingPlan = await _context.PricingPlans
        //        .FirstOrDefaultAsync(m => m.PlanID == id);
        //    if (pricingPlan == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(pricingPlan);
        //}

        //// GET: Admins/PricingPlans/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Admins/PricingPlans/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("PlanID,Title,Intro,Duration,Price,Discount")] PricingPlan pricingPlan)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(pricingPlan);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(pricingPlan);
        //}

        //// GET: Admins/PricingPlans/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var pricingPlan = await _context.PricingPlans.FindAsync(id);
        //    if (pricingPlan == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(pricingPlan);
        //}

        //// POST: Admins/PricingPlans/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("PlanID,Title,Intro,Duration,Price,Discount")] PricingPlan pricingPlan)
        //{
        //    if (id != pricingPlan.PlanID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(pricingPlan);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PricingPlanExists(pricingPlan.PlanID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(pricingPlan);
        //}

        //// GET: Admins/PricingPlans/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var pricingPlan = await _context.PricingPlans
        //        .FirstOrDefaultAsync(m => m.PlanID == id);
        //    if (pricingPlan == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(pricingPlan);
        //}

        //// POST: Admins/PricingPlans/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var pricingPlan = await _context.PricingPlans.FindAsync(id);
        //    if (pricingPlan != null)
        //    {
        //        _context.PricingPlans.Remove(pricingPlan);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool PricingPlanExists(int id)
        //{
        //    return _context.PricingPlans.Any(e => e.PlanID == id);
        //}
    }
}
