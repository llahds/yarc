using Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Data
{
    public class YARCContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<ForumOwner> ForumOwners { get; set; }
        public DbSet<ForumModerator> ForumModerator { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ReportReason> ReportReasons { get; set; }
        public DbSet<ReportedPost> ReportedPosts { get; set; }
        public DbSet<ForumMember> ForumMembers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostVote> PostVotes { get; set; }
        public DbSet<CommentVote> CommentVotes { get; set; }
        public DbSet<ReportedComment> ReportedComments { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<ForumTopic> ForumTopics { get; set; }
        public DbSet<PostScore> PostScores { get; set; }

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

            modelBuilder.Entity<ReportedPost>()
                .HasOne(F => F.ReportedBy)
                .WithMany(F => F.ReportedPosts)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Forum>()
                .Property<string>(F => F._postSettingsJson)
                .HasColumnName("PostSettings");

            modelBuilder.Entity<PostVote>()
                .HasIndex(c => new { c.PostId, c.ById })
                .IsUnique();

            modelBuilder.Entity<CommentVote>()
                .HasIndex(c => new { c.CommentId, c.ById })
                .IsUnique();

            modelBuilder.Entity<ForumTopic>()
                .HasKey(c => new { c.ForumId, c.TopicId });

            modelBuilder.Entity<PostScore>()
                .HasKey(c => new { c.PostId });

            modelBuilder.Entity<PostVote>()
                .HasIndex(e => e.CreatedOn)
                .IncludeProperties(e => e.PostId);

            modelBuilder.Entity<PostVote>()
                .HasIndex(e => e.Vote)
                .IncludeProperties(e => e.PostId);

            modelBuilder.Entity<User>().HasData(new User { Id = 1, Email = "admin", Password = "password", About = "", UserName = "admin", DisplayName = "Administrator", CreatedOn = DateTime.UtcNow });

            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 1, Label = "Breaks {forum} rules" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 2, Label = "Harassment" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 3, Label = "Threatening violence" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 4, Label = "Hate" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 5, Label = "Sexualization of minors" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 6, Label = "Sharing personal information" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 7, Label = "Non-consentual intimate media" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 8, Label = "Prohibited transaction" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 9, Label = "Impersonation" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 10, Label = "Copyright violation" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 11, Label = "Trademark violation" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 12, Label = "Self-harm or suicide" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 13, Label = "Spam" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 14, Label = "Misinformation" });

            base.OnModelCreating(modelBuilder);
        }

        public async Task SynchronizeChildren<TEntity, TChild>(Expression<Func<TEntity, bool>> query, Func<IEnumerable<int>> childIds, Func<TChild, TEntity> converter)
            where TEntity : class
            where TChild : class
        {
            var entitySet = this.Set<TEntity>();
            this.RemoveRange(await entitySet.Where(query).ToArrayAsync());

            await AddChildren(childIds, converter);
        }

        public async Task AddChildren<TEntity, TChild>(Func<IEnumerable<int>> childIds, Func<TChild, TEntity> converter)
            where TEntity : class
            where TChild : class
        {
            var entitySet = this.Set<TEntity>();
            var childSet = this.Set<TChild>();
            foreach (var id in childIds())
            {
                await entitySet.AddAsync(converter(await childSet.FindAsync(id)));
            }
        }
    }
}
