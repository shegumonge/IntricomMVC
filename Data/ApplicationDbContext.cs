using IntricomMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IntricomMVC.Data
{
    public partial class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<Hotel> Hotels { get; set; }

        public virtual DbSet<HotelBooking> HotelBookings { get; set; }

        public virtual DbSet<Configuration> Configurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.CreateDate).HasColumnType("smalldatetime");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.ToTable("Configuration");

                entity.Property(e => e.DataType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DATA_TYPE");
                entity.Property(e => e.FsFolder)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FS_FOLDER");
            });


            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("Hotel");

                entity.Property(e => e.Adress)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.CreateDate).HasColumnType("smalldatetime");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HotelBooking>(entity =>
            {
                entity.ToTable("HotelBooking");

                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.CreadeDate).HasColumnType("smalldatetime");
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Client).WithMany(p => p.HotelBookings)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelBooking_Client");

                entity.HasOne(d => d.Hotel).WithMany(p => p.HotelBookings)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelBooking_Hotel");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
