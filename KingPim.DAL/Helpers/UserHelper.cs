using KingPim.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KingPim.DAL.Helpers
{
    public enum UserRole
    {
        SuperAdmin, Admin, Publisher, Editor
    }
    public class UserHelper
    {
        IServiceProvider _serviceProvider;

        public UserHelper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task CreateRoles()
        {
            // Adding roles
            var RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "SuperAdmin", "Admin", "Publisher", "Editor" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                // Creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public async Task CreateSuperAdmin(string userEmail, string password)
        {
            await CreateUser(userEmail, password, UserRole.SuperAdmin);
        }


        public async Task CreateUser(string userEmail, string password, UserRole permission, int? catalogId = null)
        {
            string role = permission.ToString();
            var UserManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var newUser = new ApplicationUser
            {
                UserName = userEmail,
                Email = userEmail,
                CatalogId = catalogId
            };

            string UserPassword = password;
            var _user = await UserManager.FindByEmailAsync(userEmail);

            if (_user == null)
            {
                var createNewUser = await UserManager.CreateAsync(newUser, UserPassword);
                if (createNewUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(newUser, role);
                }
            }
        }
    }
}
