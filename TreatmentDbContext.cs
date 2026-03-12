using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace AllatorvosApi
{
    public class TreatmentDbContext(DbContextOptions<TreatmentDbContext> options) : DbContext(options)
    {
        public DbSet<Treatment> Treatments {  get; set; }
    }
}
