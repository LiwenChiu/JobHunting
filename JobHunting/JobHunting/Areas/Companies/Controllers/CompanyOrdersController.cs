using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobHunting.Areas.Companies.Models;

namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class CompanyOrdersController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public CompanyOrdersController(DuckCompaniesContext context)
        {
            _context = context;
        }
        //GET: Companies/CompanyOrders/Index
        public IActionResult Index()
        {
            ViewBag.CompanyOrders = new SelectList(
                    _context.CompanyOrders.Select(c => new {
                        CompanyID= c.CompanyID,
                        CompanyName= c.CompanyName,
                    }),"CompanyID","CompanyName"
                );
            return View();
        }
        //public async Task<IActionResult> CompanyOrders(int CompanyID)
        //GET: Companies/CompanyOrders/IndexJson
        public JsonResult IndexJson()
        {
            return Json(_context.CompanyOrders);
        }


        //GET: Companies/CompanyOrders
        //    public async Task<IActionResult> Index()
        //{
        //    var duckCompaniesContext = _context.CompanyOrders.Include(c => c.Company).Include(c => c.Plan);
        //    return View(await duckCompaniesContext.ToListAsync());
        //}

        // GET: Companies/CompanyOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyOrder = await _context.CompanyOrders
                .Include(c => c.Company)
                .Include(c => c.Plan)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (companyOrder == null)
            {
                return NotFound();
            }

            return View(companyOrder);
        }

        // GET: Companies/CompanyOrders/Create
        public IActionResult Create()
        {
            ViewData["CompanyID"] = new SelectList(_context.Companies, "CompanyID", "CompanyName");
            ViewData["PlanID"] = new SelectList(_context.PricingPlans, "PlanID", "Title");
            return View();
        }

        // POST: Companies/CompanyOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,CompanyID,PlanID,CompanyName,GUINumber,PlanTitle,Price,OrderDate,Status")] CompanyOrder companyOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(companyOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "CompanyID", "CompanyName", companyOrder.CompanyID);
            ViewData["PlanID"] = new SelectList(_context.PricingPlans, "PlanID", "Title", companyOrder.PlanID);
            return View(companyOrder);
        }

        // GET: Companies/CompanyOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyOrder = await _context.CompanyOrders.FindAsync(id);
            if (companyOrder == null)
            {
                return NotFound();
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "CompanyID", "CompanyName", companyOrder.CompanyID);
            ViewData["PlanID"] = new SelectList(_context.PricingPlans, "PlanID", "Title", companyOrder.PlanID);
            return View(companyOrder);
        }

        // POST: Companies/CompanyOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,CompanyID,PlanID,CompanyName,GUINumber,PlanTitle,Price,OrderDate,Status")] CompanyOrder companyOrder)
        {
            if (id != companyOrder.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyOrderExists(companyOrder.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "CompanyID", "CompanyName", companyOrder.CompanyID);
            ViewData["PlanID"] = new SelectList(_context.PricingPlans, "PlanID", "Title", companyOrder.PlanID);
            return View(companyOrder);
        }

        // GET: Companies/CompanyOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyOrder = await _context.CompanyOrders
                .Include(c => c.Company)
                .Include(c => c.Plan)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (companyOrder == null)
            {
                return NotFound();
            }

            return View(companyOrder);
        }

        // POST: Companies/CompanyOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyOrder = await _context.CompanyOrders.FindAsync(id);
            if (companyOrder != null)
            {
                _context.CompanyOrders.Remove(companyOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyOrderExists(int id)
        {
            return _context.CompanyOrders.Any(e => e.OrderID == id);
        }
    }
}
