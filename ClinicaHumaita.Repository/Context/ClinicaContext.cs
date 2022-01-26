
using ClinicaHumaita.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaHumaita.Data.Context
{
    public class ClinicaContext : DbContext
    {
        public ClinicaContext(DbContextOptions<ClinicaContext> options) : base(options)
        {
        }
        public DbSet<Person> Person { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
