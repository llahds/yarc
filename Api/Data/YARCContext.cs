using Api.Data.Entities;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

            modelBuilder.Entity<PostVote>()
                .HasIndex(e => e.CreatedOn)
                .IncludeProperties(e => e.PostId);

            modelBuilder.Entity<PostVote>()
                .HasIndex(e => e.Vote)
                .IncludeProperties(e => e.PostId);

            modelBuilder.Entity<Post>()
                .HasIndex(c => new { c.IsHidden, c.IsDeleted })
                .IncludeProperties(P => P.ForumId);

            modelBuilder.Entity<User>().HasData(new User { Id = 1, Email = "admin", Password = "password", About = "", UserName = "admin", DisplayName = "Administrator", CreatedOn = DateTime.UtcNow });
            modelBuilder.Entity<User>().HasData(new User { Id = -1, Email = "YARCBot", Password = "_________", About = "", UserName = "YARCBot", DisplayName = "YARCBot", CreatedOn = DateTime.UtcNow, IsDeleted = true });

            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 1, Label = "Breaks {forum} rules", Code = "FORUM_RULES" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 2, Label = "Harassment", Code = "HARASSMENT" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 3, Label = "Threatening violence", Code = "VIOLENCE" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 4, Label = "Hate", Code = "HATE" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 5, Label = "Sexualization of minors", Code = "SEXUALIZATION" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 6, Label = "Sharing personal information", Code = "PII" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 7, Label = "Non-consentual intimate media", Code = "NCIM" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 8, Label = "Prohibited transaction", Code = "PT" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 9, Label = "Impersonation", Code = "IMPERSONATION" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 10, Label = "Copyright violation", Code = "COPYRIGHT" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 11, Label = "Trademark violation", Code = "TRADEMARK" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 12, Label = "Self-harm or suicide", Code = "SELF_HARM" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 13, Label = "Spam", Code = "SPAM" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = 14, Label = "Misinformation", Code = "MISINFORMATION" });
            modelBuilder.Entity<ReportReason>().HasData(new ReportReason { Id = -1, Label = "Toxicity", Code = "TOXICITY" });

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
