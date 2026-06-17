using Microsoft.EntityFrameworkCore;

namespace APIPCOS_CRM.Data
{
    public class Bkmis13_Context : DbContext
    {
        public Bkmis13_Context(DbContextOptions<Bkmis13_Context> options) : base(options)
        {
        }

        public DbSet<PhieuXuatHang_HRC> PhieuXuatHang_HRCs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PhieuXuatHang_HRC>(entity =>
            {
                entity.HasKey(e => e.TicketID);
                entity.ToView("v_phieuxuathang_hrc");
            });
        }
    }
}
