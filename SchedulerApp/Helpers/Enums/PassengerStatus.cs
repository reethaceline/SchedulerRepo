using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerApp.Helpers.Enums
{
    public enum PassengerStatus
    {
        [Display(Name = "Can Schedule")]
        Schedule = 1, 
        [Display(Name = "Has Active Appointment")]
        Active, 
        [Display(Name = "Had Successful Flight")]
        SuccessfulFlight //Had a successful flight
    }
}
