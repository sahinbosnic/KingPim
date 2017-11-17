using KingPim.DAL.DataAccess;
using KingPim.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KingPim.DAL.API
{
    //[Route("api/[controller]")]
    public class ApiController : Controller
    {
        private ApplicationDbContext _ctx;
        private Catalog catalog { get; set; }

        //83891ee7-be0c-4cf5-ad86-c7b17da17def

        public ApiController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        private bool SetCatalog(string apiKey)
        {
            try
            {
                var key = _ctx.ApiKeys.Where(x => x.Key == apiKey).First();


                catalog = _ctx.Catalogs.Where(x => x.Id == key.CatalogId).First();
                catalog.Category = _ctx.Categories.Where(x => (x.CatalogId == catalog.Id) && (x.Published)).ToList();
                foreach (var category in catalog.Category)
                {
                    category.Subcategory = _ctx.Subcategories.Where(x => (x.CategoryId == category.Id) && (x.Published)).ToList();

                    foreach (var subcategory in category.Subcategory)
                    {
                        subcategory.Product = _ctx.Products.Where(x => (x.SubcategoryId == subcategory.Id) && (x.Published))
                            .ToList();

                        foreach (var product in subcategory.Product)
                        {
                            product.AttributeValue = _ctx.AttributeValues.Where(x => x.ProductId == product.Id)
                                .Include(x => x.Attribute).ToList();
                            product.SystemAttribute = _ctx.SystemAttributes.Where(x => x.Id == product.SystemAttributeId).First();
                        }
                    }
                }


                //catalog.Category = catalog.Category.Where(x => x.Published);



                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [Route("api/catalog/{apiKey}")]
        public IActionResult Catalog(string apiKey)
        {
            if (SetCatalog(apiKey))
                return new ObjectResult(catalog);

            else
                return NotFound();
        }

        [HttpGet("api/category/{apiKey}")]
        public IActionResult Category(string apiKey)
        {
            if (SetCatalog(apiKey))
                return new ObjectResult(catalog.Category);

            else
                return NotFound();
        }

        [HttpGet("api/category/{id}/{apiKey}")]
        public IActionResult Category(int id, string apiKey)
        {
            if(SetCatalog(apiKey))
                return new ObjectResult(catalog.Category.Where(x => x.Id == id));

            else
                return NotFound();
        }

        [HttpGet("api/Subcategory/{apiKey}")]
        public IActionResult Subcategory(string apiKey)
        {
            if (SetCatalog(apiKey))
            {
                var result = new List<Subcategory>();
                foreach (var category in catalog.Category)
                {
                    foreach (var subcategory in category.Subcategory)
                    {
                        result.Add(subcategory);
                    }

                }
                return new ObjectResult(result);
            }

            else
                return NotFound();
        }

        [HttpGet("api/Subcategory/{id}/{apiKey}")]
        public IActionResult Subcategory(int id, string apiKey)
        {
            if (SetCatalog(apiKey))
            {
                var result = new List<Subcategory>();
                foreach (var category in catalog.Category)
                {
                    foreach (var subcategory in category.Subcategory)
                    {
                        if(subcategory.Id == id)
                        {
                            result.Add(subcategory);
                        }
                        
                    }
                    
                }
                return new ObjectResult(result);
            }

            else
                return NotFound();
        }

        [HttpGet("api/product/{apiKey}")]
        public IActionResult Product( string apiKey)
        {
            if (SetCatalog(apiKey))
            {
                var result = new List<Product>();
                foreach (var category in catalog.Category)
                {
                    foreach (var subcategory in category.Subcategory)
                    {
                        foreach (var product in subcategory.Product)
                        {
                            result.Add(product);
                        }

                    }
                }
                return new ObjectResult(result);
            }

            else
                return NotFound();
        }

        [HttpGet("api/product/{id}/{apiKey}")]
        public IActionResult Product(int id, string apiKey)
        {
            if (SetCatalog(apiKey))
            {
                var result = new List<Product>();
                foreach (var category in catalog.Category)
                {
                    foreach (var subcategory in category.Subcategory)
                    {
                        foreach (var product in subcategory.Product)
                        {
                            if(product.Id == id)
                            {
                                result.Add(product);
                            }                            
                        }

                    }
                }
                return new ObjectResult(result);
            }

            else
                return NotFound();
        }
    }
}
