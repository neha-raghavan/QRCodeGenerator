using Microsoft.EntityFrameworkCore;

namespace QRCodeGenerater.Models
{
    public class AppDBContext : DbContext
    {

        public virtual DbSet<QRCodeData> QRCodeData { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QRCodeData>(entity =>
            {
                entity.ToTable("QRCodeData");
                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);


                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);


                entity.Property(e => e.Email)
                   .HasMaxLength(100)
                   .IsUnicode(false);


                entity.Property(e => e.Mobile)
                 .HasMaxLength(50)
                 .IsUnicode(false);
            });
        }
    }
}