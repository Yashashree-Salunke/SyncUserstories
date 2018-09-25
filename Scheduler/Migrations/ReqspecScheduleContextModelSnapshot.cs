﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Scheduler.Persistence;

namespace Scheduler.Migrations
{
    [DbContext(typeof(ReqspecScheduleContext))]
    partial class ReqspecScheduleContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ReqspecModels.Tenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessToken");

                    b.Property<string>("Code");

                    b.Property<string>("Password");

                    b.Property<string>("RepositoryUrl");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("ReqspecModels.UserstorySyncActionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.HasKey("Id");

                    b.ToTable("UserstorySyncActionTypes");
                });

            modelBuilder.Entity("ReqspecModels.UserstorySyncTracker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("TenantId");

                    b.Property<int>("UserstoryId");

                    b.Property<int>("UserstorySyncActionTypeId");

                    b.HasKey("Id");

                    b.HasIndex("UserstorySyncActionTypeId");

                    b.ToTable("UserstorySyncTrackers");
                });

            modelBuilder.Entity("Scheduler.Models.JobSyncTracker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("JobTypeId");

                    b.Property<DateTime>("LastModifiedOn");

                    b.HasKey("Id");

                    b.HasIndex("JobTypeId");

                    b.ToTable("JobSyncTrackers");
                });

            modelBuilder.Entity("Scheduler.Models.JobType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("JobTypes");
                });

            modelBuilder.Entity("ReqspecModels.UserstorySyncTracker", b =>
                {
                    b.HasOne("ReqspecModels.UserstorySyncActionType", "UserstorySyncActionType")
                        .WithMany()
                        .HasForeignKey("UserstorySyncActionTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Scheduler.Models.JobSyncTracker", b =>
                {
                    b.HasOne("Scheduler.Models.JobType", "JobType")
                        .WithMany()
                        .HasForeignKey("JobTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
