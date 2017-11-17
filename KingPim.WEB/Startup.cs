using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KingPim.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;
using KingPim.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;
using KingPim.DAL.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KingPim.WEB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbContext")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 2;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });


            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Home/Index"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });

            services.AddSingleton<IConfiguration>(Configuration);

            services
                .AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
                //.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IServiceProvider serviceProvider, ApplicationDbContext ctx)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });


            //Create default admin
            if (!userManager.Users.Any())
            {
                //Create roles
                var user = new UserHelper(serviceProvider);
                Task.WaitAll(user.CreateRoles());

                //Create admin
                var Email = Configuration.GetSection("AdminSettings")["UserEmail"];
                var Password = Configuration.GetSection("AdminSettings")["UserPassword"];
                Task.WaitAll(user.CreateSuperAdmin(Email, Password));

                //Create a default catalog
                var newCatalog = new Catalog() { Name = "Unnamed" };
                ctx.Catalogs.Add(newCatalog);
                ctx.SaveChanges();

                var catalogId = ctx.Catalogs.First();
                var getAdmin = userManager.FindByEmailAsync(Email);
                getAdmin.Result.CatalogId = catalogId.Id;
                Task.WaitAll(userManager.UpdateAsync(getAdmin.Result));

            }
        }
    }
}
