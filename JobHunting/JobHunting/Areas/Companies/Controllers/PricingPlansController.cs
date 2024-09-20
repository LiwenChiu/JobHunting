using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using System.Numerics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace JobHunting.Areas.Companies.Controllers
{
    [Authorize(Roles = "company")]
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
                PlanId = pp.PlanId,
                Title = pp.Title,
                Intro = pp.Intro,
                Duration = pp.Duration,
                Price = pp.Price,
                Discount = pp.Discount
            });
        }

        // POST: Companies/PricingPlans/PayAgree
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Array> PayAgree([FromBody][Bind("PlanId")] PayAgreementViewModel pavm )
        {
            string[] returnStatus = new string[2];

            if (!ModelState.IsValid)
            {
                returnStatus = ["輸入資料失敗", "失敗"];
                return returnStatus;
            }
            var CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(CompanyId))
            {
                returnStatus = ["未授權訪問", "失敗"];
                return returnStatus;
            }

            var company =await _context.Companies.Where(c => c.CompanyId.ToString() == CompanyId)
                .Select(c => new PayAgreementCompanyViewModel 
                {
                    CompanyId = c.CompanyId,
                    CompanyName = c.CompanyName,
                    GUINumber = c.GUINumber,
                }).SingleOrDefaultAsync();

            if (company == null)
            {
                returnStatus = ["公司資料不存在", "失敗"];
                return returnStatus;
            }

            var pricingPlan =await _context.PricingPlans.Where(pp => pp.PlanId == pavm.PlanId)
                .Select(pp => new PayAgreementPricingPlanViewModel
                {
                    PlanId = pp.PlanId,
                    Title = pp.Title,
                    Price = pp.Price,
                    Discount = pp.Discount,
                    Duration = pp.Duration,
                }).SingleOrDefaultAsync();

            if (pricingPlan == null)
            {
                returnStatus = ["方案資料不存在", "失敗"];
                return returnStatus;
            }

            CompanyOrder companyOrder = new CompanyOrder
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                GUINumber = company.GUINumber,
                PlanId = pricingPlan.PlanId,
                Title = pricingPlan.Title,
                Price = (pricingPlan.Price)*(pricingPlan.Discount),
                OrderDate = DateTime.Now,
                PayDate = DateTime.Now.AddDays(3), //在Status == false 時，PayDate當作付款期限，而當付款完成後，Status == true，PayDate再變成付款完成時間
                Duration = pricingPlan.Duration,
                Status = false,
            };

            returnStatus = ["下單成功，請於3天內付款", "成功"];
            _context.CompanyOrders.Add(companyOrder);
            await _context.SaveChangesAsync();
            return returnStatus;

        }
    }
}
