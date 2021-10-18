using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using Web4Spain.Data;
using Web4Spain.Models;

namespace Web4Spain.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, INotyfService notyf, ApplicationDbContext context)
        {
            _notyf = notyf;
            _logger = logger;
            _context = context;
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
        [Authorize]
        public IActionResult Booking()
        {
            var bookings = _context.Bookings.Where(x => x.UserId == User.Identity.Name).ToArray();
            if (bookings.Length < 1)
            {
                _notyf.Error("You have no bookings to show");

            }
            else
            {
                foreach (var x in bookings)
                {
                    Console.WriteLine(bookings.Length);
                    Console.WriteLine(x.UserId + " has booked " + ((x.ReservationEnd - x.ReservationStart).TotalDays).ToString() + " days");
                }
                ViewBag.Model = bookings;
            }

            return RedirectToAction("Booking", "Booking", null);
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
