using SchedulerApp.Helpers.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SchedulerApp.Models
{
    public class Appointment
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime AppointmentDate { get; set; }
        [Required(ErrorMessage = "*")]
        public int Capacity { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
