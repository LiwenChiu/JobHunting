using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobHunting.Areas.Admins.Models;

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
            }));
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
