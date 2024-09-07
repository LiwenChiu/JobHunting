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
using Microsoft.AspNetCore.Authentication.Cookies;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            //static DateTime Expiration(DateTime date, int DurationTime)
            //{
            //    System.TimeSpan Duration = new System.TimeSpan(DurationTime, 0, 0, 0);
            //    System.DateTime ExpirationTime = date.Add(Duration);

            //    return ExpirationTime;
            //}

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
                //ExpirationTime = Expiration(co.OrderDate, co.Plan.Duration),
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
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IEnumerable<CompanyOrdersOutputViewModel>> Filter([FromBody]CompanyOrdersViewModel covm)
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
                                    covmfilter.GUINumber.Contains(covm.GUINumber) ||
                                    covmfilter.Title.Contains(covm.Title) ||
                                    covmfilter.Intro.Contains(covm.Intro))
            .ToList()
            .Where(covmfilter => covmfilter.OrderDate.ToString() == covm.OrderDate.ToString())
              .Select(co => new CompanyOrdersOutputViewModel
               {
                   OrderID = co.OrderID,
                   CompanyID = co.CompanyID,
                   PlanID = co.PlanID,
                   CompanyName = co.CompanyName,
                   GUINumber = co.GUINumber,
                   Title = co.Title,
                   Price = co.Price,
                   OrderDate = co.OrderDate,
                   Duration = co.Duration,
                   Intro = co.Intro,
                   Status = co.Status,
               });
        }


        private bool CompanyOrderExists(int id)
        {
            return _context.CompanyOrders.Any(e => e.OrderID == id);
        }
    }
}
