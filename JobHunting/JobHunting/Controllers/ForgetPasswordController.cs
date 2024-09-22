using JobHunting.Models;
using JobHunting.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace JobHunting.Controllers
{
    public class ForgetPasswordController : Controller
    {
        private readonly DuckContext _context;
        public ForgetPasswordController(DuckContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ForgetPasswordMail()
        {
            return View();
        }




    }
}
