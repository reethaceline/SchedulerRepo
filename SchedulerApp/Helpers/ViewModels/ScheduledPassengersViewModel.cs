using SchedulerApp.Helpers.Enums;
using SchedulerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerApp.Helpers.ViewModels
{
    public class ScheduledPassengersViewModel
    {
        public DateTime AppointmentDate { get; set; }
        public int Capacity { get; set; }
        public AppointmentStatus Status { get; set; }
        public List<Passenger> Passengers { get; set; }
        public int TotalWeight { get; set; }
        public int TotalPassengers { get; set; }
    }
}
