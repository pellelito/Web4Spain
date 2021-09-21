using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Web4Spain.Models;

namespace Web4Spain.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;

        public HomeController(ILogger<HomeController> logger, INotyfService notyf)
        {
            _notyf = notyf;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost, ActionName("Index")]
        public ActionResult Index(ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                _notyf.Success("Tack, " + contact.Name);
                ModelState.Clear();

            }

            else
            {
                _notyf.Error("Something went wrong");

                return View("../Shared/_ContactModal");
            }

            return View();
        }

        public IActionResult Denia()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
