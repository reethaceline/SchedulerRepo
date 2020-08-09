using System.ComponentModel.DataAnnotations;

namespace SchedulerApp.Helpers.Enums
{
    public enum AppointmentStatus
    {
        [Display(Name = "Not Confirmed")]
        NotConfirmed = 1,
        Confirmed,
        Denied
    }
}
