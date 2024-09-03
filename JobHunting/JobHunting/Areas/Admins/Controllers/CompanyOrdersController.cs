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

        //POST: Admins/CompanyOrders/Filter
        [HttpPost("Filter")]
        [ValidateAntiForgeryToken]
        public async Task<IEnumerable<CompanyOrdersViewModel>> Filter(CompanyOrdersViewModel covm)
        {
            return _context.CompanyOrders.Include(co => co.Plan).Select(co => new CompanyOrdersViewModel
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
            }).Where(covmfilter => covmfilter.OrderID == covm.OrderID ||
                                    covmfilter.CompanyName.Contains(covm.CompanyName) ||
                                    covmfilter.GUINumber.ToString().Contains(covm.GUINumber.ToString()) ||
                                    covmfilter.Title.Contains(covm.Title) ||
                                    covmfilter.Intro.Contains(covm.Intro))
              .Select(covmfilter => new CompanyOrdersViewModel
              {
                  OrderID = covmfilter.OrderID,
                  CompanyName = covmfilter.CompanyName,
                  GUINumber = covmfilter.GUINumber,
                  Title = covmfilter.Title,
                  Intro = covmfilter.Intro,
              });
        }


        private bool CompanyOrderExists(int id)
        {
            return _context.CompanyOrders.Any(e => e.OrderID == id);
        }
    }
}
