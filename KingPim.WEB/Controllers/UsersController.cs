using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KingPim.WEB.Models;
using KingPim.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using KingPim.DAL.Models;
using KingPim.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using KingPim.DAL.Helpers;
using Microsoft.IdentityModel.Protocols;
using Attribute = KingPim.DAL.Models.Attribute;
using Microsoft.Extensions.Configuration;

namespace KingPim.WEB.Controllers
{
    [Authorize]
    [Authorize(Roles = "Administrator, SuperAdmin")]
    public class UsersController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _ctx;
        IServiceProvider _serviceProvider;
        IConfiguration _configuration;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _userManager = userManager;
            _ctx = ctx;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task<IActionResult> Users()
        {
            // Get current catalog
            var currentUser = await _userManager.GetUserAsync(User);
            var catalogId = currentUser?.CatalogId;


            var viewModel = new UsersPageViewModel();

            viewModel.Users = _ctx.Users
                .Where(x => x.CatalogId == catalogId)
                .Select(user => new UserViewModel()
                {
                    User = user,
                    Roles = _ctx.UserRoles
                        .Where(userRole => userRole.UserId == user.Id)
                        .Select(userRole => _ctx.Roles
                            .FirstOrDefault(role => userRole.RoleId == role.Id).Name).ToList()
                }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        [Route("/users/ResetPassword/{email}")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var currentUser = await _userManager.FindByEmailAsync(email);

            var result = await _userManager.RemovePasswordAsync(currentUser);
            if (result.Succeeded)
            {
                result = await _userManager.AddPasswordAsync(currentUser, _configuration.GetSection("DefaultPassword")["UserPassword"]);
            }

            return RedirectToAction(nameof(UsersController.Users), "Users");
        }

        [HttpPost]
        public async Task<IActionResult> Users(UsersPageViewModel postUser)
        {
            var formUser = new UserFormViewModel();
            formUser = postUser.Form;

            // Check if model is valid
            if (ModelState.IsValid && formUser.Email != null && formUser?.Role != null)
            {
                UserRole role = (UserRole)Enum.Parse(typeof(UserRole), formUser.Role);

                //Get current user to get current catalogId
                var currentUser = await _userManager.GetUserAsync(User);
                var catalogId = currentUser?.CatalogId;

                var newUser = new UserHelper(_serviceProvider);
                string password = "P@ssw0rd";

                Task.WaitAll(newUser.CreateUser(formUser.Email, password, role, catalogId));

                // History
                //Task.WaitAll( AddHistory("Created new user", $"Account: {postUser.Form.Email}"));
            }

            return RedirectToAction(nameof(UsersController.Users), "Users");
        }

        [HttpGet]
        [Route("/users/users/{option}/{email}")]
        public async Task<IActionResult> Users(string option, string email)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var catalogId = currentUser?.CatalogId;

            if (option == "delete")
            {
                // Delete user
                var user = await _userManager.FindByEmailAsync(email);
                await _userManager.DeleteAsync(user);
                //Task.WaitAll(AddHistory("Deleted account", $"Account: {email}"));

            }
            else if (option == "reset")
            {
                //  TODO FIX RESET PASSWORD
                //Task.WaitAll(AddHistory("Reset password", $"Account: {email}"));
                return RedirectToAction(nameof(SettingsController.Index), "Settings");
            }

            RouteData.Values.Remove("option");
            RouteData.Values.Remove("email");
            return RedirectToAction(nameof(UsersController.Users), "Users");
        }
    }
}
