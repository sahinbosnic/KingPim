using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KingPim.WEB.Models;
using Microsoft.AspNetCore.Identity;
using KingPim.DAL.Models;
using KingPim.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace KingPim.WEB.Controllers
{
    public class SettingsController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _ctx;

        public SettingsController(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx)
        {
            _userManager = userManager;
            _ctx = ctx;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var catalog = _ctx.Catalogs.Where(x => x.Id == currentUser.CatalogId)
                .Include(x => x.ApiKey)
                .First();

            var viewModel = new SettingsViewModel();
            viewModel.ApiKey = catalog.ApiKey;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SettingsViewModel settings)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if(settings.Reset != null)
            {
                var result = await _userManager.RemovePasswordAsync(currentUser);
                if (result.Succeeded)
                {
                    result = await _userManager.AddPasswordAsync(currentUser, settings.Reset.NewPassword);
                }
            }

            return RedirectToAction(nameof(SettingsController.Index), "Settings");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateApiKey()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            //Delete any old keys
            var catalog = _ctx.Catalogs.Where(x => x.Id == currentUser.CatalogId)
                .Include(x => x.ApiKey)
                .First();
            if(catalog.ApiKey != null)
            {
                catalog.ApiKey.Key = Guid.NewGuid().ToString();
                _ctx.Entry(catalog).State = EntityState.Modified;
            }
            else
            {
                //Create new
                var key = new ApiKey()
                {
                    CatalogId = currentUser.CatalogId,
                    Key = Guid.NewGuid().ToString()
                };
                _ctx.ApiKeys.Add(key);
            }
            _ctx.SaveChanges();

            return RedirectToAction(nameof(SettingsController.Index), "Settings");
        }
    }
}