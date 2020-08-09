using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchedulerApp.Data;
using SchedulerApp.Helpers.Enums;
using SchedulerApp.Helpers.ViewModels;
using SchedulerApp.Models;

namespace SchedulerApp.Controllers
{
    public class PassengerController : Controller
    {
        private readonly SchedulerContext _context;
        public PassengerController(SchedulerContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            try
            {
                var passengers = _context.Passenger.ToList();
                return View(passengers);
            }
            catch (Exception e)
            {
                var errorModel = new ErrorViewModel
                {
                    Message = e.Message
                };
                return View("Error", errorModel);
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Passenger passenger)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    passenger.Status = PassengerStatus.Schedule;
                    _context.Add(passenger);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(passenger);
            }
            catch (Exception e)
            {
                var errorModel = new ErrorViewModel
                {
                    Message = e.Message
                };
                return View("Error", errorModel);
            }
        }

        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var passenger = _context.Passenger.Where(x => x.ID == id).FirstOrDefault();
                if (passenger != null)
                {
                    return View(passenger);
                }
                return View();
            }
            catch (Exception e)
            {
                var errorModel = new ErrorViewModel
                {
                    Message = e.Message
                };
                return View("Error", errorModel);
            }
        }

        [HttpPost]
        public IActionResult Edit(Passenger passenger)
        {
            try
            {
                var _passenger = _context.Passenger.Where(x => x.ID == passenger.ID).FirstOrDefault();
                if (_passenger == null)
                {
                    return NotFound();
                }
                _passenger.FirstName = passenger.FirstName;
                _passenger.LastName = passenger.LastName;
                _passenger.Weight = passenger.Weight;
                _context.Update(_passenger);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var errorModel = new ErrorViewModel
                {
                    Message = e.Message
                };
                return View("Error", errorModel);
            }
        }
    }
}