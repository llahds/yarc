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

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Forums", "Social");
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

                    b.HasIndex("PostedById");

                    b.ToTable("Posts", "Social");
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
                            CreatedOn = new DateTime(2022, 6, 20, 14, 9, 39, 189, DateTimeKind.Utc).AddTicks(3709),
                            DisplayName = "Administrator",
                            Email = "admin",
                            Password = "password",
                            UserName = "admin"
                        });
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

            modelBuilder.Entity("Api.Data.Entities.Forum", b =>
                {
                    b.Navigation("ForumModerators");

                    b.Navigation("ForumOwners");

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Api.Data.Entities.User", b =>
                {
                    b.Navigation("ForumModerators");

                    b.Navigation("ForumOwners");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
