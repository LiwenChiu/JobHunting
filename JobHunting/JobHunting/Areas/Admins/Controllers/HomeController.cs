﻿using JobHunting.Areas.Admins.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MemberManagement()
        {
            return View();
        }

        public IActionResult CompanyList()
        {
            return View();
        }

        public IActionResult CandidateList()
        {
            return View();
        }

        public IActionResult PricingOrderHistory()
        {
            return View();
        }

        public IActionResult ClientServiceCenter()
        {
            return View();
        }

        public IActionResult PricingPlansManagement()
        {
            return View();
        }

        public IActionResult TagManagement()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TagManagement(TagManagementViewModel tmvm)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index");
            }
            return View(tmvm);
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
