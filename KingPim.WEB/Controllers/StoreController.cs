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

namespace KingPim.WEB.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _ctx;
        IServiceProvider _serviceProvider;

        public StoreController(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx, IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _ctx = ctx;
            _serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var catalogId = currentUser?.CatalogId;
            var history = _ctx.History.Where(x => x.CatalogId == catalogId).OrderByDescending(x => x.Timestamp).Take(50).ToList();

            return View(history);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public async Task<bool> AddHistory(string title, string description = "")
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var catalogId = currentUser?.CatalogId;

        //    // History
        //    var history = new History() { CatalogId = catalogId, Title = title, Description = description, User = currentUser.UserName };
        //    _ctx.History.Add(history);
        //    _ctx.SaveChanges();
        //    return true;
        //}
    }
}
