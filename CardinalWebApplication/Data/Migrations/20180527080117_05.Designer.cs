﻿// <auto-generated />
using CardinalLibrary;
using CardinalWebApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CardinalWebApplication.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180527080117_05")]
    partial class _05
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.ApplicationOption", b =>
                {
                    b.Property<int>("OptionsId")
                        .ValueGeneratedOnAdd();

                    b.Property<TimeSpan>("DataTimeWindow");

                    b.Property<string>("EndUserLicenseAgreementSource");

                    b.Property<DateTime>("OptionsDate");

                    b.Property<string>("PrivacyPolicySource");

                    b.Property<string>("TermsConditionsSource");

                    b.Property<int>("Version");

                    b.Property<int>("VersionMajor");

                    b.Property<int>("VersionMinor");

                    b.HasKey("OptionsId");

                    b.ToTable("ApplicationOptions");
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("AccountType");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<int>("Gender");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<DateTime?>("TermsAndConditionsDate");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.CurrentLayer", b =>
                {
                    b.Property<string>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LayersDelimited");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<string>("UserId1");

                    b.HasKey("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("CurrentLayers");
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.FriendGroup", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("FriendGroups");
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.FriendGroupUser", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FriendID");

                    b.Property<Guid>("GroupID");

                    b.HasKey("ID");

                    b.HasIndex("FriendID");

                    b.HasIndex("GroupID");

                    b.ToTable("FriendGroupUsers");
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.FriendRequest", b =>
                {
                    b.Property<string>("InitiatorId");

                    b.Property<string>("TargetId");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<int?>("Type");

                    b.HasKey("InitiatorId", "TargetId");

                    b.HasIndex("TargetId");

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.LocationHistory", b =>
                {
                    b.Property<Guid>("HistoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<string>("UserId");

                    b.HasKey("HistoryID");

                    b.HasIndex("UserId");

                    b.ToTable("LocationHistories");
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.Zone", b =>
                {
                    b.Property<Guid>("ZoneID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ARGBFill");

                    b.Property<string>("Description");

                    b.Property<int>("Type");

                    b.Property<string>("VisibleToLayersDelimited");

                    b.HasKey("ZoneID");

                    b.ToTable("Zones");
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.ZoneShape", b =>
                {
                    b.Property<Guid>("ZoneShapeID")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<int>("Order");

                    b.Property<Guid>("ParentZoneId");

                    b.HasKey("ZoneShapeID");

                    b.HasIndex("ParentZoneId");

                    b.ToTable("ZoneShapes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.CurrentLayer", b =>
                {
                    b.HasOne("CardinalWebApplication.Models.DbContext.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.FriendGroup", b =>
                {
                    b.HasOne("CardinalWebApplication.Models.DbContext.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.FriendGroupUser", b =>
                {
                    b.HasOne("CardinalWebApplication.Models.DbContext.ApplicationUser", "Friend")
                        .WithMany()
                        .HasForeignKey("FriendID");

                    b.HasOne("CardinalWebApplication.Models.DbContext.FriendGroup", "Group")
                        .WithMany()
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.FriendRequest", b =>
                {
                    b.HasOne("CardinalWebApplication.Models.DbContext.ApplicationUser", "Initiator")
                        .WithMany()
                        .HasForeignKey("InitiatorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CardinalWebApplication.Models.DbContext.ApplicationUser", "Target")
                        .WithMany()
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.LocationHistory", b =>
                {
                    b.HasOne("CardinalWebApplication.Models.DbContext.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CardinalWebApplication.Models.DbContext.ZoneShape", b =>
                {
                    b.HasOne("CardinalWebApplication.Models.DbContext.Zone", "ParentZone")
                        .WithMany()
                        .HasForeignKey("ParentZoneId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CardinalWebApplication.Models.DbContext.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CardinalWebApplication.Models.DbContext.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CardinalWebApplication.Models.DbContext.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CardinalWebApplication.Models.DbContext.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
