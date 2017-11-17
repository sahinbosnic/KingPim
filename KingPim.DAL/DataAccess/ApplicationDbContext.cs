using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using KingPim.DAL.Models;
using Attribute = KingPim.DAL.Models.Attribute;

namespace KingPim.DAL.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) 
            : base(options)
        {

        }

        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<AttributeGroup> AttributeGroups { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<SubCatAttributes> SubCatAttributes { get; set; }
        public DbSet<SystemAttribute> SystemAttributes { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<ProductFile> ProductFiles { get; set; }

    }
}
