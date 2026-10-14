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
                // Doi sang view_hrcproduct_v2 (14/10/2026): view cu tra NHIEU dong cho cuon co nhieu
                // mau thu -> phieu MTC lap cuon va cong du Tong so cuon (BuildResponse khong loc trung).
                entity.ToView("view_hrcproduct_v2");
            });
        }
    }
}
