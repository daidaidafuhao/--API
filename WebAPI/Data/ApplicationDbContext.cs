using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TrainingRecord> TrainingRecords { get; set; }
        public DbSet<PhotoTable> PhotoTables { get; set; }
        public DbSet<ImportHistory> ImportHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IDCardNumber).IsRequired().HasMaxLength(18);
                entity.HasIndex(e => e.IDCardNumber).IsUnique();
            });

            modelBuilder.Entity<TrainingRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.EmployeeId);
            });

            modelBuilder.Entity<PhotoTable>(entity =>
            {
                entity.HasKey(e => e.IDCardNumber);
                entity.HasOne<Employee>()
                    .WithOne()
                    .HasForeignKey<PhotoTable>(e => e.IDCardNumber)
                    .HasPrincipalKey<Employee>(e => e.IDCardNumber);
            });

            modelBuilder.Entity<ImportHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.IDCardNumber)
                    .HasPrincipalKey(e => e.IDCardNumber);
            });
        }
    }
}