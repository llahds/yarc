﻿// <auto-generated />
using System;
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(YARCContext))]
    [Migration("20220708143921_add-yarc-bot-user")]
    partial class addyarcbotuser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Social")
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Api.Data.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("PostedById")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("IsHidden");

                    b.HasIndex("ParentId");

                    b.HasIndex("PostId");

                    b.HasIndex("PostedById");

                    b.ToTable("Comments", "Social");
                });

            modelBuilder.Entity("Api.Data.Entities.CommentVote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ById")
                        .HasColumnType("int");

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Vote")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ById");

                    b.HasIndex("CommentId", "ById")
                        .IsUnique();

                    b.ToTable("CommentVotes", "Social");
                });

            modelBuilder.Entity("Api.Data.Entities.Forum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("nvarchar(125)");

                    b.Property<string>("_postSettingsJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PostSettings");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Forums", "Social");
                });

            modelBuilder.Entity("Api.Data.Entities.ForumMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("ForumId")
                        .HasColumnType("int");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ForumId");

                    b.HasIndex("MemberId");

                    b.HasIndex("Status");

                    b.ToTable("ForumMembers", "Social");
                });

            modelBuilder.Entity("Api.Data.Entities.ForumModerator", b =>
                {
                    b.Property<int>("ForumId")
                        .HasColumnType("int");

                    b.Property<int>("ModeratorId")
                        .HasColumnType("int");

                    b.HasKey("ForumId", "ModeratorId");

                    b.HasIndex("ModeratorId");

                    b.ToTable("ForumModerators", "Social");
                });

            modelBuilder.Entity("Api.Data.Entities.ForumOwner", b =>
                {
                    b.Property<int>("ForumId")
                        .HasColumnType("int");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.HasKey("ForumId", "OwnerId");

                    b.HasIndex("OwnerId");

                    b.ToTable("ForumOwners", "Social");
                });

            modelBuilder.Entity("Api.Data.Entities.ForumTopic", b =>
                {
                    b.Property<int>("ForumId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.HasKey("ForumId", "TopicId");

                    b.HasIndex("TopicId");

                    b.ToTable("ForumTopics", "Social");
                });

            modelBuilder.Entity("Api.Data.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CommentCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Downs")
                        .HasColumnType("int");

                    b.Property<int>("ForumId")
                        .HasColumnType("int");

                    b.Property<decimal>("Hot")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<decimal>("New")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PostedById")
                        .HasColumnType("int");

                    b.Property<decimal>("Rising")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<decimal>("Top")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Ups")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedOn");

                    b.HasIndex("ForumId");

                    b.HasIndex("Hot");

                    b.HasIndex("New");

                    b.HasIndex("PostedById");

                    b.HasIndex("Rising");

                    b.HasIndex("Top");

                    b.HasIndex("IsHidden", "IsDeleted");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("IsHidden", "IsDeleted"), new[] { "ForumId" });

                    b.ToTable("Posts", "Social");
                });

            modelBuilder.Entity("Api.Data.Entities.PostVote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("Vote")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ById");

                    b.HasIndex("CreatedOn");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("CreatedOn"), new[] { "PostId" });

                    b.HasIndex("Vote");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("Vote"), new[] { "PostId" });

                    b.HasIndex("PostId", "ById")
                        .IsUnique();

                    b.ToTable("PostVotes", "Social");
                });

            modelBuilder.Entity("Api.Data.Entities.ReportedComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReasonId")
                        .HasColumnType("int");

                    b.Property<int>("ReportedById")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("ReasonId");

                    b.HasIndex("ReportedById");

                    b.ToTable("ReportedComments", "Social");
                });

            modelBuilder.Entity("Api.Data.Entities.ReportedPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("ReasonId")
                        .HasColumnType("int");

                    b.Property<int>("ReportedById")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("ReasonId");

                    b.HasIndex("ReportedById");

                    b.ToTable("ReportedPosts", "Social");
                });

            modelBuilder.Entity("Api.Data.Entities.ReportReason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("ReportReasons", "Social");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Label = "Breaks {forum} rules"
                        },
                        new
                        {
                            Id = 2,
                            Label = "Harassment"
                        },
                        new
                        {
                            Id = 3,
                            Label = "Threatening violence"
                        },
                        new
                        {
                            Id = 4,
                            Label = "Hate"
                        },
                        new
                        {
                            Id = 5,
                            Label = "Sexualization of minors"
                        },
                        new
                        {
                            Id = 6,
                            Label = "Sharing personal information"
                        },
                        new
                        {
                            Id = 7,
                            Label = "Non-consentual intimate media"
                        },
                        new
                        {
                            Id = 8,
                            Label = "Prohibited transaction"
                        },
                        new
                        {
                            Id = 9,
                            Label = "Impersonation"
                        },
                        new
                        {
                            Id = 10,
                            Label = "Copyright violation"
                        },
                        new
                        {
                            Id = 11,
                            Label = "Trademark violation"
                        },
                        new
                        {
                            Id = 12,
                            Label = "Self-harm or suicide"
                        },
                        new
                        {
                            Id = 13,
                            Label = "Spam"
                        },
                        new
                        {
                            Id = 14,
                            Label = "Misinformation"
                        },
                        new
                        {
                            Id = -1,
                            Label = "Toxicity"
                        });
                });

            modelBuilder.Entity("Api.Data.Entities.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Topics", "Social");
                });

            modelBuilder.Entity("Api.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("About")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("nvarchar(125)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("IsDeleted");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users", "Social");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            About = "",
                            CreatedOn = new DateTime(2022, 7, 8, 14, 39, 20, 849, DateTimeKind.Utc).AddTicks(7257),
                            DisplayName = "Administrator",
                            Email = "admin",
                            IsDeleted = false,
                            Password = "password",
                            UserName = "admin"
                        },
                        new
                        {
                            Id = -1,
                            About = "",
                            CreatedOn = new DateTime(2022, 7, 8, 14, 39, 20, 849, DateTimeKind.Utc).AddTicks(7274),
                            DisplayName = "YARCBot",
                            Email = "YARCBot",
                            IsDeleted = true,
                            Password = "_________",
                            UserName = "YARCBot"
                        });
                });

            modelBuilder.Entity("Api.Data.Entities.Comment", b =>
                {
                    b.HasOne("Api.Data.Entities.Comment", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.HasOne("Api.Data.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Entities.User", "PostedBy")
                        .WithMany("Comments")
                        .HasForeignKey("PostedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parent");

                    b.Navigation("Post");

                    b.Navigation("PostedBy");
                });

            modelBuilder.Entity("Api.Data.Entities.CommentVote", b =>
                {
                    b.HasOne("Api.Data.Entities.User", "By")
                        .WithMany("CommentVotes")
                        .HasForeignKey("ById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Entities.Comment", "Comment")
                        .WithMany("Votes")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("By");

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("Api.Data.Entities.ForumMember", b =>
                {
                    b.HasOne("Api.Data.Entities.Forum", "Forum")
                        .WithMany("Members")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Entities.User", "Member")
                        .WithMany("Forums")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Forum");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("Api.Data.Entities.ForumModerator", b =>
                {
                    b.HasOne("Api.Data.Entities.Forum", "Forum")
                        .WithMany("ForumModerators")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Entities.User", "Moderator")
                        .WithMany("ForumModerators")
                        .HasForeignKey("ModeratorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Forum");

                    b.Navigation("Moderator");
                });

            modelBuilder.Entity("Api.Data.Entities.ForumOwner", b =>
                {
                    b.HasOne("Api.Data.Entities.Forum", "Forum")
                        .WithMany("ForumOwners")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Entities.User", "Owner")
                        .WithMany("ForumOwners")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Forum");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Api.Data.Entities.ForumTopic", b =>
                {
                    b.HasOne("Api.Data.Entities.Forum", "Forum")
                        .WithMany("Topics")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Entities.Topic", "Topic")
                        .WithMany("Forums")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Forum");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("Api.Data.Entities.Post", b =>
                {
                    b.HasOne("Api.Data.Entities.Forum", "Forum")
                        .WithMany("Posts")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Entities.User", "PostedBy")
                        .WithMany("Posts")
                        .HasForeignKey("PostedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Forum");

                    b.Navigation("PostedBy");
                });

            modelBuilder.Entity("Api.Data.Entities.PostVote", b =>
                {
                    b.HasOne("Api.Data.Entities.User", "By")
                        .WithMany("PostVotes")
                        .HasForeignKey("ById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Entities.Post", "Post")
                        .WithMany("Votes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("By");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Api.Data.Entities.ReportedComment", b =>
                {
                    b.HasOne("Api.Data.Entities.Comment", "Comment")
                        .WithMany("ReportedComments")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Entities.ReportReason", "Reason")
                        .WithMany()
                        .HasForeignKey("ReasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Entities.User", "ReportedBy")
                        .WithMany("ReportedComments")
                        .HasForeignKey("ReportedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("Reason");

                    b.Navigation("ReportedBy");
                });

            modelBuilder.Entity("Api.Data.Entities.ReportedPost", b =>
                {
                    b.HasOne("Api.Data.Entities.Post", "Post")
                        .WithMany("ReportedPosts")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Entities.ReportReason", "Reason")
                        .WithMany("ReportedPosts")
                        .HasForeignKey("ReasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Entities.User", "ReportedBy")
                        .WithMany("ReportedPosts")
                        .HasForeignKey("ReportedById")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Reason");

                    b.Navigation("ReportedBy");
                });

            modelBuilder.Entity("Api.Data.Entities.Comment", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("ReportedComments");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("Api.Data.Entities.Forum", b =>
                {
                    b.Navigation("ForumModerators");

                    b.Navigation("ForumOwners");

                    b.Navigation("Members");

                    b.Navigation("Posts");

                    b.Navigation("Topics");
                });

            modelBuilder.Entity("Api.Data.Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("ReportedPosts");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("Api.Data.Entities.ReportReason", b =>
                {
                    b.Navigation("ReportedPosts");
                });

            modelBuilder.Entity("Api.Data.Entities.Topic", b =>
                {
                    b.Navigation("Forums");
                });

            modelBuilder.Entity("Api.Data.Entities.User", b =>
                {
                    b.Navigation("CommentVotes");

                    b.Navigation("Comments");

                    b.Navigation("ForumModerators");

                    b.Navigation("ForumOwners");

                    b.Navigation("Forums");

                    b.Navigation("PostVotes");

                    b.Navigation("Posts");

                    b.Navigation("ReportedComments");

                    b.Navigation("ReportedPosts");
                });
#pragma warning restore 612, 618
        }
    }
}
