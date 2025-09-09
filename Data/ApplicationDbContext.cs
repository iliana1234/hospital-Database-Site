using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HospitalDataBase.Models;

namespace HospitalDataBase.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<HospitalDataBase.Models.Doctors> Doctors { get; set; } = default!;
        public DbSet<HospitalDataBase.Models.Patients> Patients { get; set; } = default!;
    }
}
