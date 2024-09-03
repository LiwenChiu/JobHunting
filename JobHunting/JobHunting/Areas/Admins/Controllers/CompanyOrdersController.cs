using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobHunting.Areas.Admins.Models;
using JobHunting.Areas.Admins.ViewModels;
using System.Numerics;

namespace JobHunting.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class CompanyOrdersController : Controller
    {
        private readonly DuckAdminsContext _context;

        public CompanyOrdersController(DuckAdminsContext context)
        {
            _context = context;
        }

        // GET: Admins/CompanyOrders
        public IActionResult Index()
        {
            var PricingPlan = _context.PricingPlans;
            return View(_context.CompanyOrders.Include(co => co.Plan).Select(co => new CompanyOrdersViewModel
            {
                OrderID = co.OrderID,
                CompanyID = co.CompanyID,
                PlanID = co.PlanID,
                CompanyName = co.CompanyName,
                GUINumber = co.GUINumber,
                Title = co.Title,
                Price = co.Price,
                OrderDate = co.OrderDate,
                Duration = co.Plan.Duration,
                Intro = co.Plan.Intro,
                Status = co.Status,
            }));
        }

        // GET: Admins/CompanyOrders/IndexJson
        public async Task<JsonResult> IndexJson()
        {
            return Json(_context.CompanyOrders.Include(co => co.Plan).Select(co => new CompanyOrdersViewModel
            {
                OrderID = co.OrderID,
                CompanyID = co.CompanyID,
                PlanID = co.PlanID,
                CompanyName = co.CompanyName,
                GUINumber = co.GUINumber,
                Title = co.Title,
                Price = co.Price,
                OrderDate = co.OrderDate,
                Duration = co.Plan.Duration,
                Intro = co.Plan.Intro,
                Status = co.Status,
            }));
        }


        private bool CompanyOrderExists(int id)
        {
            return _context.CompanyOrders.Any(e => e.OrderID == id);
        }
    }
}
