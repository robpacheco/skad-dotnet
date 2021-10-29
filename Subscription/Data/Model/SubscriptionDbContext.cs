using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Skad.Subscription.Data.Model
{
    public partial class SubscriptionDbContext : DbContext
    {
        public SubscriptionDbContext()
        {
        }

        public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Changelog> Changelogs { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=127.0.0.1;Database=subscription;User Id=postgres;Password=secret;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Changelog>(entity =>
            {
                entity.ToTable("changelog");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Checksum)
                    .HasMaxLength(32)
                    .HasColumnName("checksum");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.InstalledBy)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("installed_by");

                entity.Property(e => e.InstalledOn)
                    .HasColumnName("installed_on")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("name");

                entity.Property(e => e.Success).HasColumnName("success");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Version)
                    .HasMaxLength(50)
                    .HasColumnName("version");
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.ToTable("subscription");

                entity.Property(e => e.SubscriptionId).HasColumnName("subscription_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AmountPaid)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount_paid");

                entity.Property(e => e.CardLast4).HasColumnName("card_last4");

                entity.Property(e => e.CardName).HasColumnName("card_name");

                entity.Property(e => e.DateExpires)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("date_expires");

                entity.Property(e => e.DatePurchased)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("date_purchased");

                entity.Property(e => e.Tier)
                    .IsRequired()
                    .HasColumnName("tier");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
