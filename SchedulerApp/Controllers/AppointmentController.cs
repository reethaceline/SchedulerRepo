using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SchedulerApp.Data;
using SchedulerApp.Helpers.Enums;
using SchedulerApp.Helpers.ViewModels;
using SchedulerApp.Models;

namespace SchedulerApp.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly SchedulerContext _context;
        public AppointmentController(SchedulerContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            try
            {
                var appointments = _context.Appointment.ToList().OrderBy(x => x.AppointmentDate);
                return View(appointments);
            }
            catch(Exception e)
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
        public IActionResult Create(Appointment appointment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    appointment.Status = AppointmentStatus.NotConfirmed;
                    _context.Add(appointment);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
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

        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var appointment = _context.Appointment.Where(x => x.ID == id).FirstOrDefault();
                if (appointment != null)
                {
                    return View(appointment);
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
        public IActionResult Edit(Appointment appointment)
        {
            try
            {
                var _appointment = _context.Appointment.Where(x => x.ID == appointment.ID).FirstOrDefault();
                if (_appointment == null)
                {
                    return NotFound();
                }
                if (_appointment.Status == appointment.Status)
                {
                    //no status change
                    _appointment.AppointmentDate = appointment.AppointmentDate;
                    _appointment.Capacity = appointment.Capacity;
                    _context.Update(_appointment);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (_appointment.Status == AppointmentStatus.NotConfirmed && (appointment.Status == AppointmentStatus.Confirmed || appointment.Status == AppointmentStatus.Denied))
                    {
                        switch (appointment.Status)
                        {
                            case AppointmentStatus.Confirmed:
                                _appointment.AppointmentDate = appointment.AppointmentDate;
                                _appointment.Capacity = appointment.Capacity;
                                _appointment.Status = appointment.Status;
                                UpdatePassenger(appointment.ID, AppointmentStatus.Confirmed);
                                break;
                            case AppointmentStatus.Denied:
                                _appointment.AppointmentDate = appointment.AppointmentDate;
                                _appointment.Capacity = appointment.Capacity;
                                _appointment.Status = appointment.Status;
                                UpdatePassenger(appointment.ID, AppointmentStatus.Denied);
                                break;
                            case AppointmentStatus.NotConfirmed:
                                _appointment.AppointmentDate = appointment.AppointmentDate;
                                _appointment.Capacity = appointment.Capacity;
                                break;
                        }
                        _context.Update(_appointment);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var errorModel = new ErrorViewModel
                        {
                            Message = "Invalid Status Change!"
                        };
                        return View("Error", errorModel);
                    }
                }
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

        private void UpdatePassenger(int appointmentID, AppointmentStatus status)
        {
            var passengers = _context.Passenger.Where(x => x.AppointmentId == appointmentID).ToList();
            foreach (var passenger in passengers)
            {
                if (status == AppointmentStatus.Confirmed)
                {
                    passenger.Status = PassengerStatus.SuccessfulFlight;
                }
                else
                {
                    passenger.Status = PassengerStatus.Schedule;
                }
                _context.Passenger.Update(passenger);
                _context.SaveChanges();
            }
        }

        public IActionResult Schedule()
        {
            try
            {
                var passengers = _context.Passenger.Where(x => x.Status == PassengerStatus.Schedule).ToList();
                var appointments = _context.Appointment.Where(x => x.Status != AppointmentStatus.Denied).ToList();
                ViewBag.Appointments = appointments;
                var model = from passenger in passengers
                            select new PassengerViewModel
                            {
                                ID = passenger.ID,
                                FirstName = passenger.FirstName,
                                LastName = passenger.LastName,
                                Status = passenger.Status,
                                Weight = passenger.Weight
                            };
                ScheduleAppointmentViewModel scheduleAppointmentViewModel = new ScheduleAppointmentViewModel();
                scheduleAppointmentViewModel.Passengers = model.ToList();
                return View(scheduleAppointmentViewModel);
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
        public IActionResult Schedule(ScheduleAppointmentViewModel modeldata)
        {
            try
            {
                if (modeldata.Passengers != null)
                {
                    var appointment = _context.Appointment.Where(x => x.ID == modeldata.AppointmentID).FirstOrDefault();
                    if (appointment == null)
                    {
                        return NotFound();
                    }

                    //check the capacity
                    var weightTobeAdded = modeldata.Passengers.Where(x => x.IsSelected).Sum(s => s.Weight);
                    var ExistingPassengers = _context.Passenger.Where(x => x.AppointmentId == modeldata.AppointmentID).ToList();
                    var weightAdded = ExistingPassengers == null ? 0 : ExistingPassengers.Sum(s => s.Weight);

                    if (appointment.Capacity < weightTobeAdded + weightAdded)
                    {
                        var errorModel = new ErrorViewModel
                        {
                            Message = "Capacity over flow!"
                        };
                        return View("Error", errorModel);
                    }

                    foreach (var passenger in modeldata.Passengers.Where(x => x.IsSelected))
                    {
                        var _passenger = _context.Passenger.Where(x => x.ID == passenger.ID).FirstOrDefault();
                        if (_passenger != null)
                        {
                            _passenger.AppointmentId = modeldata.AppointmentID;
                            _passenger.Status = appointment.Status == AppointmentStatus.Confirmed ? PassengerStatus.SuccessfulFlight : PassengerStatus.Active;
                            _context.Update(_passenger);
                            _context.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorModel = new ErrorViewModel
                    {
                        Message = "Passengers Should be selected before scheduling!"
                    };
                    return View("Error", errorModel);
                }
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
        public IActionResult Details(int ID)
        {
            try
            {
                var appointment = _context.Appointment.Where(x => x.ID == ID).FirstOrDefault();
                var passengers = _context.Passenger.Where(x => x.AppointmentId == ID);
                var totalWeight = passengers.Sum(x => x.Weight);
                if (appointment != null)
                {
                    var modeldata = new ScheduledPassengersViewModel()
                    {
                        AppointmentDate = appointment.AppointmentDate,
                        Capacity = appointment.Capacity,
                        Status = appointment.Status,
                        Passengers = passengers.ToList(),
                        TotalPassengers = passengers.ToList().Count,
                        TotalWeight = totalWeight
                    };
                    return View(modeldata);
                }
                return View(new ScheduledPassengersViewModel());
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