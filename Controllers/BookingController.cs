﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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
            booking.ReservationStart = booking.ReservationStart.AddHours(16);
            booking.ReservationEnd = booking.ReservationEnd.AddHours(11);
            // Console.WriteLine("Booking: " + booking.ReservationStart);
            if (ModelState.IsValid && CheckAvailability(booking))
            {
                booking.BookingId = Guid.NewGuid();
                booking.HousingId = 1;
                booking.Status = 1;
                booking.UserId = User.Identity.Name;


                //add booking to db
                _context.Bookings.Add(booking);
                _context.SaveChanges();
                ModelState.Clear();
                AddToCalender(1, booking);

            }
            else
            {
                _notyf.Error("Something went wrong");


            }

            return RedirectToAction(nameof(Booking));
        }

        private static void AddToCalender(int action, BookingModel booking)

        {
            String calendarId = @"v0sp8kc55n687i5d7rv0f0uqko@group.calendar.google.com";
            string jsonFile = "web4spain-22b04057a032.json";
            string[] Scopes = { CalendarService.Scope.Calendar };
            var eventId = (booking.BookingId.ToString().Replace("-", ""));
            

            //connect to goggle calendar
            ServiceAccountCredential credential;

            using (var stream =
                new FileStream(jsonFile, FileMode.Open, FileAccess.Read))
            {
                var confg = Google.Apis.Json.NewtonsoftJsonSerializer.Instance.Deserialize<JsonCredentialParameters>(stream);
                credential = new ServiceAccountCredential(
                   new ServiceAccountCredential.Initializer(confg.ClientEmail)
                   {
                       Scopes = Scopes
                   }.FromPrivateKey(confg.PrivateKey));
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Web4Spain",
            });

            
            Event calendarEvent = new()
            {
                Start = new EventDateTime
                {
                    DateTime = booking.ReservationStart,
                    TimeZone = "Europe/Stockholm"
                },
                End = new EventDateTime
                {
                    DateTime = booking.ReservationEnd,
                    TimeZone = "Europe/Stockholm"
                },
                Id = eventId,
                Summary = "Booked"

            };

            switch (action)
            {
                case 1:
                    //insert new event i google calender       
                    service.Events.Insert(calendarEvent, calendarId).Execute();
                    break;

                case 2:
                    //delete from google calendar
                    service.Events.Delete(calendarId, eventId).Execute();
                    break;

                case 3:
                    //edit calender
                    service.Events.Update(calendarEvent, calendarId, eventId).Execute();
                    break;

            }
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
            booking.ReservationStart = booking.ReservationStart.AddHours(16);
            booking.ReservationEnd = booking.ReservationEnd.AddHours(11);

            if (CheckAvailability(booking))
            {
                _context.Update(booking);
                _context.SaveChanges();
                AddToCalender(3, booking);
                return RedirectToAction(nameof(Booking));

            }
            else
            {
                return View();
            }


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
                AddToCalender(2, booking);
                _context.Remove(booking);
                _context.SaveChanges();
                return RedirectToAction(nameof(Booking));
            }
            catch
            {
                return View();
            }
        }
        public bool CheckAvailability(BookingModel booking)
        {
            int check = ((int)(booking.ReservationEnd - booking.ReservationStart).TotalDays) + 1;
            Console.WriteLine(check % 7);

            if ((check % 7 != 0) || (booking.ReservationStart.DayOfWeek != DayOfWeek.Saturday))
            {
                _notyf.Error("Please only select whole weeks, from Saturday to Saturday");
                return false;

            }

            var bookings = _context.Bookings.AsNoTracking().ToArray();
            if (bookings.Length > 0)
            {
                foreach (var x in bookings)
                {
                    if (!x.BookingId.Equals(booking.BookingId))
                    {
                        if (((x.ReservationStart <= booking.ReservationStart) && (booking.ReservationStart <= x.ReservationEnd)) || (x.ReservationStart <= booking.ReservationEnd) && (booking.ReservationEnd <= x.ReservationEnd))

                        {
                            _notyf.Error("Not avalible on selected dates");
                            return false;
                        }
                    }

                }
            }

            return true;
        }
    }
}