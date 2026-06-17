using APIPCOS_CRM.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace APIPCOS_CRM.Data
{
    public class PLCOS_Context: DbContext
    {
        private readonly IConfiguration _config;
        public PLCOS_Context()
        {
        }

        public PLCOS_Context(DbContextOptions<PLCOS_Context> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public virtual DbSet<VESSELS> VESSELS { get; set; } = null!;
        public virtual DbSet<VESSELINFOR> VESSELINFOR { get; set; } = null!;
        public virtual DbSet<VESSEL_SIZE> VESSEL_SIZE { get; set; } = null!;
        public DbSet<VwVessel> vw_vessels { get; set; }
        public DbSet<User> USER { get; set; }
        public DbSet<VOYAGEPORTS> VOYAGEPORTS { get; set; }
        public DbSet<ScheduleBerthModel> ScheduleBerth { get; set; }

        public IQueryable<ScheduleBerthModel> GetByDate(DateTime fromDate,DateTime toDate)
        {
            SqlParameter fromDay = new SqlParameter("@fromDay", fromDate);
            SqlParameter toDay = new SqlParameter("@toDay", toDate);
            return this.ScheduleBerth.FromSqlRaw($"sp_schedule_berth @fromDay,@toDay", fromDay, toDay);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _config.GetConnectionString("DB");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VESSELS>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("VESSELS");
                entity.Property(e => e.VESSEL_ID).HasMaxLength(20).HasColumnName("VESSEL_ID");
                entity.Property(e => e.VESSEL_NAME).HasMaxLength(30).HasColumnName("VESSEL_NAME");
                entity.Property(e => e.NET_WEIGHT).HasColumnType("decimal(26, 8)").HasColumnName("NET_WEIGHT");
                entity.Property(e => e.GROSS_WEIGHT).HasColumnType("decimal(26, 8)").HasColumnName("GROSS_WEIGHT");
                entity.Property(e => e.DEAD_WEIGHT).HasColumnType("decimal(26, 8)").HasColumnName("DEAD_WEIGHT");
                entity.Property(e => e.LOA).HasColumnType("decimal(23, 8)").HasColumnName("LOA");
                entity.Property(e => e.LBP).HasColumnType("decimal(23, 8)").HasColumnName("LBP");
                entity.Property(e => e.BEAM).HasColumnType("decimal(23, 8)").HasColumnName("BEAM");
                entity.Property(e => e.VESSEL_SIZE_ID).HasMaxLength(20).HasColumnName("VESSEL_SIZE_ID");
                entity.Property(e => e.UPDATE_STAFF).HasMaxLength(25).HasColumnName("UPDATE_STAFF");
                entity.Property(e => e.UPDATE_TIME).HasColumnType("datetime").HasColumnName("UPDATE_TIME");
                entity.Property(e => e.VESSEL_TYPE2).HasMaxLength(20);
                entity.Property(e => e.CALL_SIGN).HasMaxLength(20);
                entity.Property(e => e.IMO).HasMaxLength(40);

            });
      
            modelBuilder.Entity<VESSEL_SIZE>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("VESSEL_SIZE");
                entity.Property(e => e.VESSEL_SIZE_ID).HasMaxLength(20).HasColumnName("VESSEL_SIZE_ID");
                entity.Property(e => e.VESSEL_SIZE_NAME).HasMaxLength(30).HasColumnName("VESSEL_SIZE_NAME");
                entity.Property(e => e.FROM_CARGO_WEIGHT).HasColumnType("decimal(26, 8)").HasColumnName("FROM_CARGO_WEIGHT");
                entity.Property(e => e.TO_CARGO_WEIGHT).HasColumnType("decimal(26, 8)").HasColumnName("TO_CARGO_WEIGHT");
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USERS_API");
                entity.Property(e => e.username).HasMaxLength(20).HasColumnName("username");
                entity.Property(e => e.password).HasMaxLength(255).HasColumnName("password");
                entity.Property(e => e.status).HasColumnType("int").HasColumnName("status");

            });
            modelBuilder.Entity<VOYAGEPORTS>(entity =>
            {
                entity.ToTable("VOYAGEPORTS");
                entity.HasNoKey();
                entity.Property(e => e.ROW_ID).HasColumnName("ROW_ID");
                entity.Property(e => e.UPDATE_TIME).HasColumnType("datetime").HasColumnName("UPDATE_TIME");
                entity.Property(e => e.UPDATE_STAFF).HasMaxLength(50).HasColumnName("UPDATE_STAFF");
                entity.Property(e => e.CREATE_STAFF).HasMaxLength(50).HasColumnName("CREATE_STAFF");
                entity.Property(e => e.CREATE_TIME).HasColumnType("datetime").HasColumnName("CREATE_TIME");
                entity.Property(e => e.LOADING_PORT).HasMaxLength(6).HasColumnName("LOADING_PORT");
                entity.Property(e => e.DISCHARGE_PORT).HasMaxLength(6).HasColumnName("DISCHARGE_PORT");
                entity.Property(e => e.SO).HasMaxLength(50).HasColumnName("SO");
                entity.Property(e => e.ETA).HasColumnType("datetime").HasColumnName("ETA");
                entity.Property(e => e.ROW_ID_BERTHPLAN).HasColumnName("ROW_ID_BERTHPLAN");

            });




            modelBuilder.Entity<ScheduleBerthModel>().HasNoKey().ToView(null);
       
        modelBuilder.Entity<VwVessel>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_vessels");

                entity.Property(e => e.Beam)
                    .HasColumnType("decimal(23, 8)")
                    .HasColumnName("BEAM");

                entity.Property(e => e.DeadWeight)
                    .HasColumnType("decimal(26, 8)")
                    .HasColumnName("DEAD_WEIGHT");

                entity.Property(e => e.FromCargoWeight)
                    .HasColumnType("decimal(28, 8)")
                    .HasColumnName("FROM_CARGO_WEIGHT");

                entity.Property(e => e.GrossWeight)
                    .HasColumnType("decimal(26, 8)")
                    .HasColumnName("GROSS_WEIGHT");

                entity.Property(e => e.Lbp)
                    .HasColumnType("decimal(23, 8)")
                    .HasColumnName("LBP");

                entity.Property(e => e.Loa)
                    .HasColumnType("decimal(23, 8)")
                    .HasColumnName("LOA");

                entity.Property(e => e.NetWeight)
                    .HasColumnType("decimal(26, 8)")
                    .HasColumnName("NET_WEIGHT");

                entity.Property(e => e.ToCargoWeight)
                    .HasColumnType("decimal(28, 8)")
                    .HasColumnName("TO_CARGO_WEIGHT");

                entity.Property(e => e.UpdateStaff)
                    .HasMaxLength(25)
                    .HasColumnName("UPDATE_STAFF");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_TIME");

                entity.Property(e => e.VesselId)
                    .HasMaxLength(20)
                    .HasColumnName("VESSEL_ID");

                entity.Property(e => e.VesselName)
                    .HasMaxLength(30)
                    .HasColumnName("VESSEL_NAME");

                entity.Property(e => e.VesselSizeId)
                    .HasMaxLength(20)
                    .HasColumnName("VESSEL_SIZE_ID");

                entity.Property(e => e.VesselSizeName)
                    .HasMaxLength(50)
                    .HasColumnName("VESSEL_SIZE_NAME");
            });
        }
    }
}
