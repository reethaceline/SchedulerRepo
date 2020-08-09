using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchedulerApp.Helpers.ViewModels
{
    public class ScheduleAppointmentViewModel
    {
        [Display(Name = "Appointment Date")]
        public int AppointmentID { get; set; }
        public List<PassengerViewModel> Passengers { get; set; }
    }
}
