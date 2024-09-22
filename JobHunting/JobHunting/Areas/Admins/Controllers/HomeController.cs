﻿using JobHunting.Areas.Admins.ViewModels;
using JobHunting.Areas.Candidates.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace JobHunting.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class HomeController : Controller
    {
        private readonly DuckCandidatesContext _context;

        public HomeController(DuckCandidatesContext context)
        {
            _context = context;
        }
        [Authorize(AuthenticationSchemes = "AdminScheme", Roles = "admin")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(AuthenticationSchemes = "AdminScheme", Roles = "admin")]
        public IActionResult MemberManagement()
        {
            return View();
        }
       
        public IActionResult Login()
        {
            return View();
        }

         

    }
}
