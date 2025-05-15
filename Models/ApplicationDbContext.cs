using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PROG7311_PART_2.Models
{
    /// <summary>
    /// The application's database context, including Identity and domain entities.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// Gets or sets the products in the system.
        public DbSet<Product> Products { get; set; }

        /// Gets or sets the farmers in the system.
        public DbSet<Farmer> Farmers { get; set; }

        /// Configure relationships
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Farmer>()
    .HasOne(f => f.User)
    .WithOne()
    .HasForeignKey<Farmer>(f => f.UserId)
    .IsRequired(false); // allow null if no linked user

        }
    }
}
