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
    public class CatalogController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _ctx;
        IServiceProvider _serviceProvider;

        public CatalogController(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx, IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _ctx = ctx;
            _serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Catalog()
        {
            // Get current user id too find correct catalog
            var currentUser = await _userManager.GetUserAsync(User);
            var viewModel = new CatalogViewModel();
            viewModel.Catalog = _ctx.Catalogs.Find(currentUser.CatalogId);

            //Get categories
            viewModel.Categories = _ctx.Categories.Where(x => x.CatalogId == currentUser.CatalogId)
                .Include(x => x.Subcategory)
                    .ThenInclude(x => x.SubCatAttributes)
                .ToList();
            viewModel.AttributeGroups = _ctx.AttributeGroups.Where(x => x.CatalogId == currentUser.CatalogId).ToList();

            /* 
                .Include(x => x.Quotes.Select(q => q.QuoteItems))
                .Where(x => x.JobID == id)
             */
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Catalog(CatalogViewModel postCatalog)
        {
            if (ModelState.IsValid)
            {

                // Catalog has been changed
                if (postCatalog.Catalog != null)
                {
                    _ctx.Entry(postCatalog.Catalog).State = EntityState.Modified;
                    _ctx.SaveChanges();
                    //Task.WaitAll(AddHistory("Updated catalog"));

                }
                // A form has been submited
                else if (postCatalog.Form != null)
                {
                    //Create new category
                    if (postCatalog.Form.CategoryName != null)
                    {
                        var form = postCatalog.Form;
                        var newCategory = new Category();

                        newCategory.CatalogId = form.CatalogId;
                        newCategory.Name = form.CategoryName;
                        newCategory.Published = form.CategoryPublished;
                        _ctx.Categories.Add(newCategory);
                        _ctx.SaveChanges();
                        //Task.WaitAll(AddHistory("Created new category"));
                    }
                    //Create new subcategory
                    else if (postCatalog.Form.SubcategoryName != null)
                    {
                        var form = postCatalog.Form;
                        var newSub = new Subcategory();
                        newSub.CategoryId = form.CategoryId;
                        newSub.Published = form.SubcategoryPublished;
                        newSub.Name = form.SubcategoryName;
                        List<SubCatAttributes> atrGroup = new List<SubCatAttributes>();
                        foreach (var id in form.GroupId)
                        {
                            atrGroup.Add(new SubCatAttributes { SubcategoryId = newSub.Id, AttributeGroupId = id });
                            //atrGroup.Add(_ctx.AttributeGroups.Where(x => x.Id == id).First());
                        }
                        //newSub.SubCatAttributes = atrGroup;
                        newSub.SubCatAttributes = atrGroup;

                        _ctx.Subcategories.Add(newSub);
                        _ctx.SaveChanges();
                        //Task.WaitAll(AddHistory("Created new subcategory"));
                    }
                }
            }
            
            return RedirectToAction(nameof(CatalogController.Catalog), "Catalog");
        }
        
        // TODO secure against user injection
        [HttpGet]
        [Route("/catalog/catalog/{option}/{type}/{id}")]
        public IActionResult Catalog(string option, string type, int id)
        {
            if (type == "subcategory")
            {
                var subCategory = _ctx.Subcategories.Find(id);
                if (option == "delete")
                {
                    // TODO Remove attributegroup before deleting subcat
                    /*subCategory.AttributeGroup = null;
                    _ctx.Entry(subCategory).State = EntityState.Modified;
                    _ctx.Entry(subCategory.AttributeGroup).State = EntityState.Modified;
                    _ctx.SaveChanges();*/
                    _ctx.Subcategories.Remove(subCategory);
                }
                else //Toggle publish
                {
                    if(subCategory.Published) { subCategory.Published = false; }
                    else { subCategory.Published = true; }
                    _ctx.Entry(subCategory).State = EntityState.Modified;
                    //Task.WaitAll(AddHistory("Toggled publishstate on category"));
                }
            }
            else
            {
                var category = _ctx.Categories.Find(id);
                if (option == "delete")
                {
                    _ctx.Categories.Remove(category);
                    //Task.WaitAll(AddHistory("Removed subcategory"));
                }
                else //Toggle publish
                {
                    if (category.Published) { category.Published = false; }
                    else { category.Published = true; }
                    _ctx.Entry(category).State = EntityState.Modified;
                    //Task.WaitAll(AddHistory("Toggled publishstate on subcategory"));
                }
            }

            _ctx.SaveChanges();

            RouteData.Values.Remove("option");
            RouteData.Values.Remove("type");
            RouteData.Values.Remove("id");
            return RedirectToAction(nameof(CatalogController.Catalog), "Catalog");
        }

    }
}
