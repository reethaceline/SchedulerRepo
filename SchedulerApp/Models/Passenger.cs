using SchedulerApp.Helpers.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerApp.Models
{
    [Table("Passenger")]
    public class Passenger
    {
        public int ID { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }
        [Display(Name = "Weight")]
        [Required(ErrorMessage = "*")]
        public int Weight { get; set; }
        public PassengerStatus Status { get; set; }
        public int? AppointmentId { get; set; }
    }
}
