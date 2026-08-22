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
                // KHONG dung HasKey(TicketID): 1 phieu xuat chua nhieu cuon, nen TicketID bi lap
                // tren view (do ~3.8 dong/ticket). Khai bao khoa khien EF Core gop cac dong cung
                // TicketID lai thanh 1 entity qua identity map -> mat phan lon du lieu cuon.
                entity.HasNoKey();
                entity.ToView("v_phieuxuathang_hrc");
            });
        }
    }
}
