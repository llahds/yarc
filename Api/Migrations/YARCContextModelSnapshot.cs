﻿// <auto-generated />
using System;
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(YARCContext))]
    partial class YARCContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("PostedById")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("PostId");

                    b.HasIndex("PostedById");

                    b.ToTable("Comments", "Social");
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

            modelBuilder.Entity("Api.Data.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("ForumId")
                        .HasColumnType("int");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<int>("PostedById")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedOn");

                    b.HasIndex("ForumId");

                    b.HasIndex("IsHidden");

                    b.HasIndex("PostedById");

                    b.ToTable("Posts", "Social");
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
                        });
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

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users", "Social");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            About = "",
                            CreatedOn = new DateTime(2022, 6, 21, 23, 42, 31, 114, DateTimeKind.Utc).AddTicks(3712),
                            DisplayName = "Administrator",
                            Email = "admin",
                            Password = "password",
                            UserName = "admin"
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
                });

            modelBuilder.Entity("Api.Data.Entities.Forum", b =>
                {
                    b.Navigation("ForumModerators");

                    b.Navigation("ForumOwners");

                    b.Navigation("Members");

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Api.Data.Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("ReportedPosts");
                });

            modelBuilder.Entity("Api.Data.Entities.ReportReason", b =>
                {
                    b.Navigation("ReportedPosts");
                });

            modelBuilder.Entity("Api.Data.Entities.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("ForumModerators");

                    b.Navigation("ForumOwners");

                    b.Navigation("Forums");

                    b.Navigation("Posts");

                    b.Navigation("ReportedPosts");
                });
#pragma warning restore 612, 618
        }
    }
}
