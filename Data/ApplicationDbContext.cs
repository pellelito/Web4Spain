using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web4Spain.Models;

namespace Web4Spain.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUserModel>
    {
       

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUserModel> ApplicationUserModels { get; set; }
        public DbSet<BookingModel> Bookings { get; set; }



    }


}
