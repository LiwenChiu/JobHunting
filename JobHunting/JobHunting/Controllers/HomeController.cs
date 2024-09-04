using JobHunting.Areas.Candidates.ViewModels;
using JobHunting.Areas.Companies.ViewModel;
using JobHunting.Models;
using JobHunting.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SQLitePCL;
using System;
using System.Diagnostics;
using System.Net;

namespace JobHunting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
                         DuckContext _context;
        public HomeController(ILogger<HomeController> logger, DuckContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexTest()
        {
            return Json(_context.Candidates.Select(c => new Candidate
            {
                Name = c.Name,
                Sex = c.Sex,
                Degree = c.Degree,
                Address = c.Address,
            }));

        }

        public IActionResult Privacy()
        {
            return View();
        }
        //public IActionResult CompanyIndex()
        //{
        //    return View();
        //}

        public async Task<IActionResult> CompanyIndex()
        {
            return View(await _context.Candidates.ToListAsync());
        }
        public async Task<IActionResult> TagList()
        {
            var Tag = _context.Tags;
            var TagClass = _context.TagClasses;

            return View(Tag.Select(p => new ResumeViewModel
            {
                TagName = p.TagName,
                TagClass = TagClass.Where(s => s.TagClassID == p.TagClassID).Select(s => s.TagClass).Single(),
            }));

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
