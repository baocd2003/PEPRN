using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Repository.Models
{
    public partial class WatercolorsPainting2024DBContext : DbContext
    {
        public WatercolorsPainting2024DBContext()
        {
        }

        public WatercolorsPainting2024DBContext(DbContextOptions<WatercolorsPainting2024DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Style> Styles { get; set; } = null!;
        public virtual DbSet<UserAccount> UserAccounts { get; set; } = null!;
        public virtual DbSet<WatercolorsPainting> WatercolorsPaintings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            return configuration.GetConnectionString("ConnectionStrings");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Style>(entity =>
            {
                entity.ToTable("Style");

                entity.Property(e => e.StyleId).HasMaxLength(30);

                entity.Property(e => e.OriginalCountry).HasMaxLength(160);

                entity.Property(e => e.StyleDescription).HasMaxLength(250);

                entity.Property(e => e.StyleName).HasMaxLength(100);
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("UserAccount");

                entity.HasIndex(e => e.UserEmail, "UQ__UserAcco__08638DF8318921AC")
                    .IsUnique();

                entity.Property(e => e.UserAccountId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserAccountID");

                entity.Property(e => e.UserEmail).HasMaxLength(90);

                entity.Property(e => e.UserFullName).HasMaxLength(70);

                entity.Property(e => e.UserPassword).HasMaxLength(50);
            });

            modelBuilder.Entity<WatercolorsPainting>(entity =>
            {
                entity.HasKey(e => e.PaintingId)
                    .HasName("PK__Watercol__CF2D90F2B4CB72AF");

                entity.ToTable("WatercolorsPainting");

                entity.Property(e => e.PaintingId).HasMaxLength(45);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.PaintingAuthor).HasMaxLength(120);

                entity.Property(e => e.PaintingDescription).HasMaxLength(250);

                entity.Property(e => e.PaintingName).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.StyleId).HasMaxLength(30);

                entity.HasOne(d => d.Style)
                    .WithMany(p => p.WatercolorsPaintings)
                    .HasForeignKey(d => d.StyleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Watercolo__Style__286302EC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
