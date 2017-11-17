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

namespace KingPim.WEB.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _ctx;
        IServiceProvider _serviceProvider;
        private readonly IHostingEnvironment _appEnvironment;

        public ProductsController(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx, IServiceProvider serviceProvider, IHostingEnvironment appEnvironment)
        {
            _userManager = userManager;
            _ctx = ctx;
            _serviceProvider = serviceProvider;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var viewModel = new ProductViewModel();
            viewModel.Products = _ctx.Products
                .Where(x => x.Subcategory.Category.CatalogId == currentUser.CatalogId)
                .Include(x => x.Subcategory.Category)
                .Include(x => x.Subcategory.SubCatAttributes)
                    .ThenInclude(x => x.AttributeGroup)
                .Include(x => x.SystemAttribute)
                .ToList();

            return View(viewModel);
        }

        [HttpGet]
        [Route("/products/index/{option}/{id}")]
        public IActionResult Index(string option, int id)
        {
            // TODO SECURE
            var product = _ctx.Products.Find(id);
            if (option == "delete")
            {
                _ctx.Products.Remove(product);
            }
            else if (option == "publish")
            {
                if (product.Published)
                {
                    product.Published = false;
                    product.SystemAttribute.Published = false;
                }
                else {
                    product.Published = true;
                    product.SystemAttribute.Published = true;
                }
                _ctx.Entry(product).State = EntityState.Modified;
            }
            _ctx.SaveChanges();

            RouteData.Values.Remove("option");
            RouteData.Values.Remove("id");

            return RedirectToAction(nameof(ProductsController.Index), "Products");
        }

        public async Task<IActionResult> Add()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var viewModel = new ProductAddViewModel();
            viewModel.Subcategories = _ctx.Subcategories
                .Where(x => x.Category.CatalogId == currentUser.CatalogId)
                .Include(x => x.Category)
                .ToList();
            //viewModel.AttributeGroup = _ctx.AttributeGroups.Where(x => x.CatalogId == currentUser.CatalogId).ToList();

            return View(viewModel);
        }

        [HttpGet]
        [Route("/products/add/{id}")]
        public async Task<IActionResult> Add(int id)
        {
            // TODO snygga till...
            var currentUser = await _userManager.GetUserAsync(User);

            var viewModel = new ProductAddViewModel();
            viewModel.Product = _ctx.Products.Where(x => x.Id == id).Include(x => x.SystemAttribute).First();
            viewModel.Product.ProductFiles = _ctx.ProductFiles.Where(x => x.ProductId == id).ToList();
            viewModel.Product.Subcategory = _ctx.Subcategories.FirstOrDefault(x => x.Id == viewModel.Product.SubcategoryId);
            viewModel.Product.Subcategory.SubCatAttributes = _ctx.SubCatAttributes.Where(x => x.SubcategoryId == viewModel.Product.Subcategory.Id).ToList();

            foreach (var item in viewModel.Product.Subcategory.SubCatAttributes)
            {
                item.AttributeGroup = _ctx.AttributeGroups.FirstOrDefault(x => x.Id == item.AttributeGroupId);
                item.AttributeGroup.Attribute = _ctx.Attributes.Where(x => x.AttributeGroupId == item.AttributeGroup.Id).ToList();
                foreach (var val in item.AttributeGroup.Attribute)
                {
                    val.AttributeValue = _ctx.AttributeValues.Where(x => x.AttributeId == val.Id && x.ProductId == viewModel.Product.Id).ToList();
                    if (!val.AttributeValue.Any())
                    {
                        val.AttributeValue.Add(new AttributeValue()
                        {
                            ProductId = id,
                            Product = viewModel.Product,
                            AttributeId = val.Id,
                            Attribute = val,
                            AttributeGroupId = item.AttributeGroup.Id,
                            AttributeGroup = item.AttributeGroup
                        });
                    }
                }
            }

            viewModel.Subcategories = _ctx.Subcategories.Where(x => x.Category.CatalogId == currentUser.CatalogId).ToList();


            viewModel.SubCatAttributes = viewModel.Product.Subcategory.SubCatAttributes;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductAddViewModel postProduct)
        {
            // TODO Split view into 2 forms
            var currentUser = await _userManager.GetUserAsync(User);

            if (postProduct.MediaType > 0)
            {
                postProduct.Product.SystemAttribute = _ctx.SystemAttributes.Find(postProduct.Product.SystemAttributeId);
                postProduct.Product.SystemAttribute.LastModified = DateTime.Now;

                _ctx.Entry(postProduct.Product.SystemAttribute).State = EntityState.Modified;

                var file = _ctx.ProductFiles.Where(x => x.Id == postProduct.FileId).First();
                file.MediaType = postProduct.MediaType;
                _ctx.SaveChanges();
                return RedirectToAction(nameof(ProductsController.Add), new { id = postProduct.ReturnId });
            }
            else if (postProduct.Product.Id > 0)
            {
                //Post already exists, update it 
                postProduct.Product.SystemAttribute = _ctx.SystemAttributes.Find(postProduct.Product.SystemAttributeId);
                postProduct.Product.SystemAttribute.LastModified = DateTime.Now;
                postProduct.Product.SystemAttribute.Version++;
                postProduct.Product.SystemAttribute.ModifiedBy = currentUser.NormalizedUserName;

                _ctx.Entry(postProduct.Product.SystemAttribute).State = EntityState.Modified;
                _ctx.Entry(postProduct.Product).State = EntityState.Modified;
                _ctx.SaveChanges();

                //Stay on product page
                return RedirectToAction(nameof(ProductsController.Add), new { id = postProduct.Product.Id });
            }
            else if (postProduct.Product.Id == 0)
            {
                //Create new
                var system = new SystemAttribute();
                system.Created = DateTime.Now;
                system.Version = 0;

                if(postProduct.Product.Published) { system.Published = true; }
                else { system.Published = false; }


                _ctx.SystemAttributes.Add(system);
                _ctx.SaveChanges();

                postProduct.Product.SystemAttributeId = system.Id;
                //postProduct.Product.SystemAttribute = system;
                _ctx.Products.Add(postProduct.Product);
                _ctx.SaveChanges();

                // TODO Redirect to newly created product
                return RedirectToAction(nameof(ProductsController.Add), new { id = postProduct.Product.Id });
            }

            // If we get here something is wrong
            return RedirectToAction(nameof(ProductsController.Add));
        }

        [HttpGet]
        [Route("/products/add/{option}/{id}/{returnId}")]
        public IActionResult Add(string option, int id, int returnId)
        {
            // TODO SECURE
            var file = _ctx.ProductFiles.Find(id);
            if (option == "delete")
            {
                _ctx.ProductFiles.Remove(file);
            }
            else if (option == "publish")
            {
                if (file.Published)
                {
                    file.Published = false;
                }
                else { file.Published = true; }
                _ctx.Entry(file).State = EntityState.Modified;
            }
            else if (option == "primary")
            {
                if (file.MainFile)
                {
                    file.MainFile = false;
                }
                else { file.MainFile = true; }
                _ctx.Entry(file).State = EntityState.Modified;
            }

            _ctx.SaveChanges();

            RouteData.Values.Remove("option");
            RouteData.Values.Remove("type");
            RouteData.Values.Remove("id");
            RouteData.Values.Remove("returnId");

            return RedirectToAction(nameof(ProductsController.Add), new { id = returnId });
        }

        [HttpPost]
        public async Task<IActionResult> AddAttributes(ProductAddViewModel postProduct)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                //_ctx.SubCatAttributes.UpdateRange(postProduct.SubCatAttributes);
                foreach (var item in postProduct.SubCatAttributes)
                {
                    foreach (var val in item.AttributeGroup.Attribute)
                    {
                        if (val.AttributeValue[0].Id == 0)
                        {
                            _ctx.AttributeValues.Add(val.AttributeValue[0]);
                        }
                        else
                        {
                            _ctx.AttributeValues.Update(val.AttributeValue[0]);
                        }
                    }

                }
                postProduct.Product.SystemAttribute = _ctx.SystemAttributes.Find(postProduct.Product.SystemAttributeId);
                postProduct.Product.SystemAttribute.LastModified = DateTime.Now;
                postProduct.Product.SystemAttribute.Version++;
                postProduct.Product.SystemAttribute.ModifiedBy = currentUser.NormalizedUserName;

                _ctx.Entry(postProduct.Product.SystemAttribute).State = EntityState.Modified;
                _ctx.SaveChanges();
            }
            return RedirectToAction(nameof(ProductsController.Add), new { id = postProduct.Product.Id });
        }

        [HttpPost]
        public async Task<IActionResult> AddFiles(ICollection<IFormFile> files, string fileName, int id)
        {
            foreach (var formFile in files)
            {
                var fileType = formFile.ContentType.Split("/")[1];

                switch (fileType)
                {
                    case "jpg":
                    case "jpeg":
                        fileType = ".jpg";
                        break;

                    case "png":
                        fileType = ".png";
                        break;

                    case "text":
                    case "plain":
                        fileType = ".txt";
                        break;

                    case "mp4":
                        fileType = ".mp4";
                        break;

                    case "pdf":
                        fileType = ".pdf";
                        break;

                    default:
                        fileType = "";
                        break;
                }

                var fileGuid = Guid.NewGuid();
                var filePath = _appEnvironment.WebRootPath + "/uploads/" + fileGuid + fileType;
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);

                        var file = new ProductFile()
                        {
                            ProductId = id,
                            Name = fileName,
                            Type = fileType.Substring(1),
                            GuidName = fileGuid.ToString(),
                            MainFile = false,
                            Published = false,
                            Created = DateTime.Now
                        };

                        _ctx.ProductFiles.Add(file);
                        _ctx.SaveChanges();
                    }
                }
            }
            return RedirectToAction(nameof(ProductsController.Add), new { id = id });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
