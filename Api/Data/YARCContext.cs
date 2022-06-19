using Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class YARCContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<ForumOwner> ForumOwners { get; set; }
        public DbSet<ForumModerator> ForumModerator { get; set; }
        public DbSet<Post> Posts { get; set; }

        public YARCContext(DbContextOptions<YARCContext> options)
            : base(options)
        {

        }

        public YARCContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                var config = builder.Build();

                optionsBuilder.UseSqlServer(config["connectionStrings:db"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Social");

            modelBuilder.Entity<ForumModerator>()
                .HasKey(c => new { c.ForumId, c.ModeratorId });

            modelBuilder.Entity<ForumOwner>()
                .HasKey(c => new { c.ForumId, c.OwnerId });

            modelBuilder.Entity<User>().HasData(new User { Id = 1, Email = "admin", Password = "password", About = "", DisplayName = "admin", CreatedOn = DateTime.UtcNow });

            base.OnModelCreating(modelBuilder);
        }
    }
}
