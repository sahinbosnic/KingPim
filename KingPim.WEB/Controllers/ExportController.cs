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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using Newtonsoft.Json;
using KingPim.DAL.API;
using System.Net.Http.Headers;

namespace KingPim.WEB.Controllers
{
    [Authorize]
    public class ExportController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _ctx;
        IServiceProvider _serviceProvider;
        private readonly IHostingEnvironment _appEnvironment;

        public ExportController(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx, IServiceProvider serviceProvider, IHostingEnvironment appEnvironment)
        {
            _userManager = userManager;
            _ctx = ctx;
            _serviceProvider = serviceProvider;
            _appEnvironment = appEnvironment;
        }

        [Route("Export/Index")]
        [Route("Export/Index/{guid}")]
        [HttpGet]
        public async Task<IActionResult> Index(string guid = null)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var viewModel = new ExportViewModel();
            viewModel.Catalog = _ctx.Catalogs.Where(x => x.Id == currentUser.CatalogId).First();
            viewModel.Category = _ctx.Categories.Where(x => x.CatalogId == viewModel.Catalog.Id).ToList();
            viewModel.Subcategory = _ctx.Subcategories.Where(x => x.Category.CatalogId == viewModel.Catalog.Id).ToList();
            viewModel.Product = _ctx.Products.Where(x => x.Subcategory.Category.CatalogId == viewModel.Catalog.Id).ToList();

            if(guid != null)
            {
                //there is an exported file, get it
                viewModel.FileGuid = guid;
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ExportViewModel exportModel)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var key = _ctx.ApiKeys.Where(x => x.CatalogId == currentUser.CatalogId).First();

            string result = "";
            System.Net.WebClient wc = new System.Net.WebClient();
            var host = "http://" + Request.Host;

            if (exportModel.getCatalog > 0)
            {
                result = wc.DownloadString(host + "/Api/Catalog/" + key.Key);
            }

            else if (exportModel.getCategory > 0)
            {
                result = wc.DownloadString(host + "/Api/Category/" + exportModel.getCategory + "/" + key.Key);
            }

            else if (exportModel.getSubcategory > 0)
            {
                result = wc.DownloadString(host + "/Api/Subcategory/" + exportModel.getSubcategory + "/" + key.Key);
            }
            else if (exportModel.getProduct > 0)
            {
                result = wc.DownloadString(host + "/Api/Product/" + exportModel.getProduct + "/" + key.Key);
            }


            var fileName = Guid.NewGuid().ToString() +".json";

            System.IO.File.WriteAllText(_appEnvironment.WebRootPath + "/uploads/" + fileName, result);

            return RedirectToAction(nameof(ExportController.Index), new { guid = fileName });
        }
    }
}
