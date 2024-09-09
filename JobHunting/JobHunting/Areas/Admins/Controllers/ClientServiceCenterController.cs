using JobHunting.Areas.Admins.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class ClientServiceCenterController : Controller
    {
        private readonly DuckAdminsContext _context;

        public ClientServiceCenterController(DuckAdminsContext context)
        {
            _context = context;
        }
        // GET: ClientServiceCenterController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ClientServiceCenterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientServiceCenterController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientServiceCenterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientServiceCenterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClientServiceCenterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientServiceCenterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClientServiceCenterController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
