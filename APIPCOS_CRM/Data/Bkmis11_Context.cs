using Microsoft.EntityFrameworkCore;

namespace APIPCOS_CRM.Data
{
    public class Bkmis11_Context : DbContext
    {
        public Bkmis11_Context(DbContextOptions<Bkmis11_Context> options) : base(options)
        {
        }

        public DbSet<HRC_Product> HRC_Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HRC_Product>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("view_hrcproduct");
            });
        }
    }
}
