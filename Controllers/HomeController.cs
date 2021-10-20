﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public ActionResult Index(string name, string email, string message)
        {
            if (ModelState.IsValid)
            {
                // send contact form
                _notyf.Success("Tack, " + name);
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
            ViewBag.Model = bookings;
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
