﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StoryForce.Server.Data;

namespace StoryForce.Server.Migrations
{
    [DbContext(typeof(PgDbContext))]
    [Migration("20210628045826_InitSchema")]
    partial class InitSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CategoryStoryFile", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("integer");

                    b.Property<int>("StoryFilesId")
                        .HasColumnType("integer");

                    b.HasKey("CategoriesId", "StoryFilesId");

                    b.HasIndex("StoryFilesId");

                    b.ToTable("CategoryStoryFile");
                });

            modelBuilder.Entity("PersonStoryFile", b =>
                {
                    b.Property<int>("FeaturedPeopleId")
                        .HasColumnType("integer");

                    b.Property<int>("FeaturedStoryFileId")
                        .HasColumnType("integer");

                    b.HasKey("FeaturedPeopleId", "FeaturedStoryFileId");

                    b.HasIndex("FeaturedStoryFileId");

                    b.ToTable("PersonStoryFile");
                });

            modelBuilder.Entity("PersonSubmission", b =>
                {
                    b.Property<int>("FeaturedPeopleId")
                        .HasColumnType("integer");

                    b.Property<int>("FeaturedSubmissionsId")
                        .HasColumnType("integer");

                    b.HasKey("FeaturedPeopleId", "FeaturedSubmissionsId");

                    b.HasIndex("FeaturedSubmissionsId");

                    b.ToTable("PersonSubmission");
                });

            modelBuilder.Entity("StoryFileTag", b =>
                {
                    b.Property<int>("StoryFilesId")
                        .HasColumnType("integer");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer");

                    b.HasKey("StoryFilesId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("StoryFileTag");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.AuditDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("StoryFileId")
                        .HasColumnType("integer");

                    b.Property<int?>("SubmissionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("StoryFileId");

                    b.HasIndex("SubmissionId");

                    b.ToTable("AuditDetails");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("StoryFileId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("StoryFileId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("Category")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("StoryFileId")
                        .HasColumnType("integer");

                    b.Property<int?>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("StoryFileId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("StoryFileId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("StoryFileId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text");

                    b.Property<int?>("ClassOfYear")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.Property<int?>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Story", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Stories");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.StoryFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ApprovedSubmissionId")
                        .HasColumnType("integer");

                    b.Property<int?>("Class")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("DownloadUrl")
                        .HasColumnType("text");

                    b.Property<int?>("EventId")
                        .HasColumnType("integer");

                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<List<string>>("Keywords")
                        .HasColumnType("text[]");

                    b.Property<int?>("RejectedSubmissionId")
                        .HasColumnType("integer");

                    b.Property<int?>("RequestedById")
                        .HasColumnType("integer");

                    b.Property<long?>("Size")
                        .HasColumnType("bigint");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("SubmissionId")
                        .HasColumnType("integer");

                    b.Property<int?>("SubmittedById")
                        .HasColumnType("integer");

                    b.Property<string>("ThumbnailUrl")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ApprovedSubmissionId");

                    b.HasIndex("EventId");

                    b.HasIndex("RejectedSubmissionId");

                    b.HasIndex("RequestedById");

                    b.HasIndex("SubmissionId");

                    b.HasIndex("SubmittedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("StoryFiles");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Submission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ApprovedById")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("EventId")
                        .HasColumnType("integer");

                    b.Property<int?>("ReviewedById")
                        .HasColumnType("integer");

                    b.Property<int?>("SubmittedById")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ApprovedById");

                    b.HasIndex("EventId");

                    b.HasIndex("ReviewedById");

                    b.HasIndex("SubmittedById");

                    b.ToTable("Submissions");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("StoryStoryFile", b =>
                {
                    b.Property<int>("BelongsToId")
                        .HasColumnType("integer");

                    b.Property<int>("FilesId")
                        .HasColumnType("integer");

                    b.HasKey("BelongsToId", "FilesId");

                    b.HasIndex("FilesId");

                    b.ToTable("StoryStoryFile");
                });

            modelBuilder.Entity("CategoryStoryFile", b =>
                {
                    b.HasOne("StoryForce.Shared.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoryForce.Shared.Models.StoryFile", null)
                        .WithMany()
                        .HasForeignKey("StoryFilesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PersonStoryFile", b =>
                {
                    b.HasOne("StoryForce.Shared.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("FeaturedPeopleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoryForce.Shared.Models.StoryFile", null)
                        .WithMany()
                        .HasForeignKey("FeaturedStoryFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PersonSubmission", b =>
                {
                    b.HasOne("StoryForce.Shared.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("FeaturedPeopleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoryForce.Shared.Models.Submission", null)
                        .WithMany()
                        .HasForeignKey("FeaturedSubmissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StoryFileTag", b =>
                {
                    b.HasOne("StoryForce.Shared.Models.StoryFile", null)
                        .WithMany()
                        .HasForeignKey("StoryFilesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoryForce.Shared.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StoryForce.Shared.Models.AuditDetail", b =>
                {
                    b.HasOne("StoryForce.Shared.Models.StoryFile", null)
                        .WithMany("History")
                        .HasForeignKey("StoryFileId");

                    b.HasOne("StoryForce.Shared.Models.Submission", null)
                        .WithMany("History")
                        .HasForeignKey("SubmissionId");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Category", b =>
                {
                    b.HasOne("StoryForce.Shared.Models.Category", "ParentCategory")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentCategoryId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Comment", b =>
                {
                    b.HasOne("StoryForce.Shared.Models.StoryFile", null)
                        .WithMany("Comments")
                        .HasForeignKey("StoryFileId");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Event", b =>
                {
                    b.HasOne("StoryForce.Shared.Models.StoryFile", null)
                        .WithMany("Events")
                        .HasForeignKey("StoryFileId");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Note", b =>
                {
                    b.HasOne("StoryForce.Shared.Models.StoryFile", null)
                        .WithMany("Notes")
                        .HasForeignKey("StoryFileId");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.StoryFile", b =>
                {
                    b.HasOne("StoryForce.Shared.Models.Submission", "ApprovedSubmission")
                        .WithMany("ApprovedFiles")
                        .HasForeignKey("ApprovedSubmissionId");

                    b.HasOne("StoryForce.Shared.Models.Event", "Event")
                        .WithMany("StoryFiles")
                        .HasForeignKey("EventId");

                    b.HasOne("StoryForce.Shared.Models.Submission", "RejectedSubmission")
                        .WithMany("RejectedFiles")
                        .HasForeignKey("RejectedSubmissionId");

                    b.HasOne("StoryForce.Shared.Models.Person", "RequestedBy")
                        .WithMany("RequestedStoryFiles")
                        .HasForeignKey("RequestedById");

                    b.HasOne("StoryForce.Shared.Models.Submission", "Submission")
                        .WithMany("SubmittedFiles")
                        .HasForeignKey("SubmissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoryForce.Shared.Models.Person", "SubmittedBy")
                        .WithMany("SubmittedStoryFiles")
                        .HasForeignKey("SubmittedById");

                    b.HasOne("StoryForce.Shared.Models.Person", "UpdatedBy")
                        .WithMany("UpdatedStoryFiles")
                        .HasForeignKey("UpdatedById");

                    b.Navigation("ApprovedSubmission");

                    b.Navigation("Event");

                    b.Navigation("RejectedSubmission");

                    b.Navigation("RequestedBy");

                    b.Navigation("Submission");

                    b.Navigation("SubmittedBy");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Submission", b =>
                {
                    b.HasOne("StoryForce.Shared.Models.Person", "ApprovedBy")
                        .WithMany("ApprovedSubmissions")
                        .HasForeignKey("ApprovedById");

                    b.HasOne("StoryForce.Shared.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.HasOne("StoryForce.Shared.Models.Person", "ReviewedBy")
                        .WithMany("ReviewedBySubmissions")
                        .HasForeignKey("ReviewedById");

                    b.HasOne("StoryForce.Shared.Models.Person", "SubmittedBy")
                        .WithMany("SubmittedSubmissions")
                        .HasForeignKey("SubmittedById");

                    b.Navigation("ApprovedBy");

                    b.Navigation("Event");

                    b.Navigation("ReviewedBy");

                    b.Navigation("SubmittedBy");
                });

            modelBuilder.Entity("StoryStoryFile", b =>
                {
                    b.HasOne("StoryForce.Shared.Models.Story", null)
                        .WithMany()
                        .HasForeignKey("BelongsToId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoryForce.Shared.Models.StoryFile", null)
                        .WithMany()
                        .HasForeignKey("FilesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Category", b =>
                {
                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Event", b =>
                {
                    b.Navigation("StoryFiles");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Person", b =>
                {
                    b.Navigation("ApprovedSubmissions");

                    b.Navigation("RequestedStoryFiles");

                    b.Navigation("ReviewedBySubmissions");

                    b.Navigation("SubmittedStoryFiles");

                    b.Navigation("SubmittedSubmissions");

                    b.Navigation("UpdatedStoryFiles");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.StoryFile", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Events");

                    b.Navigation("History");

                    b.Navigation("Notes");
                });

            modelBuilder.Entity("StoryForce.Shared.Models.Submission", b =>
                {
                    b.Navigation("ApprovedFiles");

                    b.Navigation("History");

                    b.Navigation("RejectedFiles");

                    b.Navigation("SubmittedFiles");
                });
#pragma warning restore 612, 618
        }
    }
}
