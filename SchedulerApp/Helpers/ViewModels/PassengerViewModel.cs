using SchedulerApp.Helpers.Enums;
using SchedulerApp.Models;
using System;

namespace SchedulerApp.Helpers.ViewModels
{
    public class PassengerViewModel : Passenger
    {
        public bool IsSelected { get; set; }
    }
}
