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
    public class AttributesController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _ctx;
        IServiceProvider _serviceProvider;

        public AttributesController(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx, IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _ctx = ctx;
            _serviceProvider = serviceProvider;
        }


        public async Task<IActionResult> Attributes()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var catalogId = currentUser?.CatalogId;

            var viewModel = new AttributeViewModel();

            viewModel.AttributeGroup = _ctx.AttributeGroups.Where(x => x.CatalogId == catalogId).Include(x => x.Attribute).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Attributes(AttributeViewModel postAttribute)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var catalogId = currentUser?.CatalogId;

            // Check if model is valid
            if (ModelState.IsValid)
            {
                var form = postAttribute.Form;
                //Create new group
                if (form.NewGroup != null)
                {
                    var group = new AttributeGroup() { CatalogId = catalogId, Name = form.NewGroup };
                    _ctx.AttributeGroups.Add(group);
                    _ctx.SaveChanges();
                    //Task.WaitAll(AddHistory("Added new Attribute Group", form.NewGroup));
                }
                else if (form.SelectGroup != null && form.AttributeName != null /*&& form.ValueType != null*/)
                {
                    var attribute = new Attribute() { AttributeGroupId = form.SelectGroup, Name = form.AttributeName, ValueType = form.ValueType};
                    _ctx.Attributes.Add(attribute);
                    _ctx.SaveChanges();
                    //Task.WaitAll(AddHistory("Added new Attribute", form.AttributeName));
                }
            }

            return RedirectToAction(nameof(AttributesController.Attributes), "Attributes");
        }

        // TODO secure against user injection
        [HttpGet]
        [Route("/attributes/attributes/{option}/{type}/{id}")]
        public IActionResult Attributes(string option, string type, int id)
        {
            if (type == "group")
            {
                if(option == "delete")
                {
                    var group = _ctx.AttributeGroups.Find(id);
                    _ctx.AttributeGroups.Remove(group);
                    //Task.WaitAll(AddHistory("Deleted AttributeGroup", group.Name));
                }
            }
            else if (type == "attribute")
            {
                if (option == "delete")
                {
                    var attribute = _ctx.Attributes.Find(id);
                    _ctx.Attributes.Remove(attribute);
                    //Task.WaitAll(AddHistory("Deleted Attribute", attribute.Name));
                }
            }

            _ctx.SaveChanges();

            RouteData.Values.Remove("option");
            RouteData.Values.Remove("type");
            RouteData.Values.Remove("id");
            return RedirectToAction(nameof(AttributesController.Attributes), "Attributes");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<bool> AddHistory(string title, string description = "")
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var catalogId = currentUser?.CatalogId;

            // History
            var history = new History() { CatalogId = catalogId, Title = title, Description = description, User = currentUser.UserName };
            _ctx.History.Add(history);
            _ctx.SaveChanges();
            return true;
        }
    }
}
