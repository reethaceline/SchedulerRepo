using Microsoft.EntityFrameworkCore;
using SchedulerApp.Models;
using SchedulerApp.Helpers.ViewModels;

namespace SchedulerApp.Data
{
    public class SchedulerContext : DbContext
    {
        public SchedulerContext(DbContextOptions<SchedulerContext> options) : base(options)
        {

        }
        public DbSet<Passenger> Passenger { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
    }
}
