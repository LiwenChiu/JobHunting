using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;

namespace JobHunting.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class PricingPlansController : Controller
    {
        private readonly DuckCompaniesContext _context;

        public PricingPlansController(DuckCompaniesContext context)
        {
            _context = context;
        }

        // GET: Companies/PricingPlans
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // POST: Companies/PricingPlans/BootFilterPage
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<PricingPlanOutputViewModel>> BootFilterPage([FromBody][Bind("PlanId,Title,Intro,Duration,Price,Discount,Status")] PricingPlanViewModel ppvm)
        {
            return _context.PricingPlans.Select(pp => new PricingPlanViewModel
            {
                PlanId = pp.PlanId,
                Title = pp.Title,
                Intro = pp.Intro,
                Duration = pp.Duration,
                Price = pp.Price,
                Discount = pp.Discount,
                Status = pp.Status
            }).Where(ppvmfilter => (ppvmfilter.Title.Contains(ppvm.Title) || 
                                    ppvmfilter.Intro.Contains(ppvm.Intro) || 
                                    ppvmfilter.Duration.ToString().Contains(ppvm.Duration.ToString()) ||
                                    ppvmfilter.Price.ToString().Contains(ppvm.Price.ToString()) ||
                                    ppvmfilter.Discount.ToString().Contains(ppvm.Discount.ToString())) && ppvmfilter.Status == true)
            .Select(pp => new PricingPlanOutputViewModel
            {
                Title = pp.Title,
                Intro = pp.Intro,
                Duration = pp.Duration,
                Price = pp.Price,
                Discount = pp.Discount
            });
        }

        //// POST: Companies/PricingPlans/PayAgree
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public async Task<> PayAgree([FromHeader])
        //{

        //}
    }
}
