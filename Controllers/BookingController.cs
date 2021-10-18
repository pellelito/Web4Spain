using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Web4Spain.Data;
using Web4Spain.Models;

namespace Web4Spain.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        // GET: BookingController
        private readonly ILogger<BookingController> _logger;
        private readonly INotyfService _notyf;
        private readonly ApplicationDbContext _context;

        public BookingController(ILogger<BookingController> logger, INotyfService notyf, ApplicationDbContext context)
        {
            _notyf = notyf;
            _logger = logger;
            _context = context;
        }

        public ActionResult Booking()
        {
            var bookings = _context.Bookings.Where(x => x.UserId == User.Identity.Name).ToArray();
            ViewBag.Model = bookings;
            return View();
        }

        [HttpPost]
        public ActionResult Booking(BookingModel booking)
        {

            if (ModelState.IsValid && checkAvailability(booking))
            {
                booking.BookingId = Guid.NewGuid();
                booking.HousingId = 1;
                booking.Status = 1;
                booking.UserId = User.Identity.Name;
                _notyf.Success($"You successfully requested a booking for " +
               $"{(booking.ReservationEnd - booking.ReservationStart).TotalDays} days, arriving " +
               $"{booking.ReservationStart.ToShortDateString()} and leaving " +
               $"{booking.ReservationEnd.ToShortDateString()}");

                _context.Bookings.Add(booking);
                _context.SaveChanges();
                ModelState.Clear();


            }
            else
            {
                _notyf.Error("Something went wrong");


            }

            return RedirectToAction(nameof(Booking));
        }
        // GET: BookingController/Details/5
        public ActionResult Details(Guid id)
        {
            BookingModel booking = _context.Bookings.Where(x => x.BookingId == id).FirstOrDefault();
            return View(booking);
        }



        // GET: BookingController/Edit/5
        public ActionResult Edit(Guid id)
        {
            BookingModel booking = _context.Bookings.Where(x => x.BookingId == id).FirstOrDefault();
            return View(booking);
        }

        // POST: BookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, BookingModel booking)
        {
            try
            {
                var bookings = _context.Bookings.ToArray();
                _notyf.Success("Uppdated booking starting: <br>" + booking.ReservationStart.ToShortDateString() + "<br> and ending: <br>" + booking.ReservationEnd.ToShortDateString());
                _context.Update(booking);
                _context.SaveChanges();
            }
            catch
            {
                _notyf.Error("Unable to proceed with booking");
            }
            return RedirectToAction(nameof(Booking));
        }

        // GET: BookingController/Delete/5
        public ActionResult Delete(Guid id)
        {

            BookingModel booking = _context.Bookings.Where(x => x.BookingId == id).FirstOrDefault();
            return View(booking);
        }

        // POST: BookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, BookingModel booking)
        {
            try
            {
                _context.Remove(booking);
                _context.SaveChanges();
                return RedirectToAction(nameof(Booking));
            }
            catch
            {
                return View();
            }
        }
        public bool checkAvailability(BookingModel booking)
        {
            var bookings = _context.Bookings.ToArray();
            if (bookings.Length > 1)
            {
                foreach (var x in bookings)
                {
                    if (((x.ReservationStart <= booking.ReservationStart) && (booking.ReservationStart <= x.ReservationEnd)) || (x.ReservationStart <= booking.ReservationEnd) && (booking.ReservationEnd <= x.ReservationEnd))

                    {
                        _notyf.Error("Not avalible on selected dates");
                        return false;
                    }

                }
            }
            return true;
        }
    }
}