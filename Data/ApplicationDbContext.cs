using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace PiikkiTracker.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<UserJob> UserJobs { get; set; }
        public DbSet<UserProduct> UserProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Job>()
                .Property(j => j.Payment)
                .HasPrecision(18, 2);


            modelBuilder.Entity<UserJob>()
                .HasKey(uj => uj.Id);

            modelBuilder.Entity<UserProduct>()
                .HasKey(up => up.Id);


            modelBuilder.Entity<UserJob>()
                .HasOne(uj => uj.User)
                .WithMany(u => u.UserJobs)
                .HasForeignKey(uj => uj.UserId);

            modelBuilder.Entity<UserJob>()
                .HasOne(uj => uj.Job)
                .WithMany(j => j.UserJobs)
                .HasForeignKey(uj => uj.JobId);


            modelBuilder.Entity<UserProduct>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProducts)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserProduct>()
                .HasOne(up => up.Product)
                .WithMany(p => p.UserProducts)
                .HasForeignKey(up => up.ProductId);



        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
