﻿// <auto-generated />
using KingPim.DAL.DataAccess;
using KingPim.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace KingPim.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20171113122631_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KingPim.DAL.Models.ApiKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CatalogId");

                    b.Property<string>("Key");

                    b.HasKey("Id");

                    b.HasIndex("CatalogId")
                        .IsUnique()
                        .HasFilter("[CatalogId] IS NOT NULL");

                    b.ToTable("ApiKeys");
                });

            modelBuilder.Entity("KingPim.DAL.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int?>("ApiKeyId");

                    b.Property<int?>("CatalogId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

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

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ApiKeyId");

                    b.HasIndex("CatalogId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("KingPim.DAL.Models.Attribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AttributeGroupId");

                    b.Property<string>("Name");

                    b.Property<int>("ValueType");

                    b.HasKey("Id");

                    b.HasIndex("AttributeGroupId");

                    b.ToTable("Attributes");
                });

            modelBuilder.Entity("KingPim.DAL.Models.AttributeGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CatalogId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("AttributeGroups");
                });

            modelBuilder.Entity("KingPim.DAL.Models.AttributeValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AttributeGroupId");

                    b.Property<int>("AttributeId");

                    b.Property<int>("ProductId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("AttributeGroupId");

                    b.HasIndex("AttributeId");

                    b.HasIndex("ProductId");

                    b.ToTable("AttributeValues");
                });

            modelBuilder.Entity("KingPim.DAL.Models.Catalog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Catalogs");
                });

            modelBuilder.Entity("KingPim.DAL.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CatalogId");

                    b.Property<string>("Name");

                    b.Property<bool>("Published");

                    b.HasKey("Id");

                    b.HasIndex("CatalogId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("KingPim.DAL.Models.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CatalogId");

                    b.Property<string>("Description");

                    b.Property<DateTime>("Timestamp");

                    b.Property<string>("Title");

                    b.Property<string>("User");

                    b.HasKey("Id");

                    b.ToTable("History");
                });

            modelBuilder.Entity("KingPim.DAL.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<bool>("Published");

                    b.Property<int>("SubcategoryId");

                    b.Property<int>("SystemAttributeId");

                    b.HasKey("Id");

                    b.HasIndex("SubcategoryId");

                    b.HasIndex("SystemAttributeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("KingPim.DAL.Models.ProductFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("GuidName");

                    b.Property<bool>("MainFile");

                    b.Property<int>("MediaType");

                    b.Property<string>("Name");

                    b.Property<int>("ProductId");

                    b.Property<bool>("Published");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductFiles");
                });

            modelBuilder.Entity("KingPim.DAL.Models.SubCatAttributes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttributeGroupId");

                    b.Property<int>("SubcategoryId");

                    b.HasKey("Id");

                    b.HasIndex("AttributeGroupId");

                    b.HasIndex("SubcategoryId");

                    b.ToTable("SubCatAttributes");
                });

            modelBuilder.Entity("KingPim.DAL.Models.Subcategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<string>("Name");

                    b.Property<bool>("Published");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Subcategories");
                });

            modelBuilder.Entity("KingPim.DAL.Models.SystemAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("ModifiedBy");

                    b.Property<bool>("Published");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("SystemAttributes");
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

            modelBuilder.Entity("KingPim.DAL.Models.ApiKey", b =>
                {
                    b.HasOne("KingPim.DAL.Models.Catalog", "Catalog")
                        .WithOne("ApiKey")
                        .HasForeignKey("KingPim.DAL.Models.ApiKey", "CatalogId");
                });

            modelBuilder.Entity("KingPim.DAL.Models.ApplicationUser", b =>
                {
                    b.HasOne("KingPim.DAL.Models.ApiKey", "ApiKey")
                        .WithMany()
                        .HasForeignKey("ApiKeyId");

                    b.HasOne("KingPim.DAL.Models.Catalog", "Catalog")
                        .WithMany()
                        .HasForeignKey("CatalogId");
                });

            modelBuilder.Entity("KingPim.DAL.Models.Attribute", b =>
                {
                    b.HasOne("KingPim.DAL.Models.AttributeGroup", "AttributeGroup")
                        .WithMany("Attribute")
                        .HasForeignKey("AttributeGroupId");
                });

            modelBuilder.Entity("KingPim.DAL.Models.AttributeValue", b =>
                {
                    b.HasOne("KingPim.DAL.Models.AttributeGroup", "AttributeGroup")
                        .WithMany()
                        .HasForeignKey("AttributeGroupId");

                    b.HasOne("KingPim.DAL.Models.Attribute", "Attribute")
                        .WithMany("AttributeValue")
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KingPim.DAL.Models.Product", "Product")
                        .WithMany("AttributeValue")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KingPim.DAL.Models.Category", b =>
                {
                    b.HasOne("KingPim.DAL.Models.Catalog", "Catalog")
                        .WithMany("Category")
                        .HasForeignKey("CatalogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KingPim.DAL.Models.Product", b =>
                {
                    b.HasOne("KingPim.DAL.Models.Subcategory", "Subcategory")
                        .WithMany("Product")
                        .HasForeignKey("SubcategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KingPim.DAL.Models.SystemAttribute", "SystemAttribute")
                        .WithMany()
                        .HasForeignKey("SystemAttributeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KingPim.DAL.Models.ProductFile", b =>
                {
                    b.HasOne("KingPim.DAL.Models.Product")
                        .WithMany("ProductFiles")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KingPim.DAL.Models.SubCatAttributes", b =>
                {
                    b.HasOne("KingPim.DAL.Models.AttributeGroup", "AttributeGroup")
                        .WithMany("SubCatAttributes")
                        .HasForeignKey("AttributeGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KingPim.DAL.Models.Subcategory", "Subcategory")
                        .WithMany("SubCatAttributes")
                        .HasForeignKey("SubcategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KingPim.DAL.Models.Subcategory", b =>
                {
                    b.HasOne("KingPim.DAL.Models.Category", "Category")
                        .WithMany("Subcategory")
                        .HasForeignKey("CategoryId")
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
                    b.HasOne("KingPim.DAL.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("KingPim.DAL.Models.ApplicationUser")
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

                    b.HasOne("KingPim.DAL.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("KingPim.DAL.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
