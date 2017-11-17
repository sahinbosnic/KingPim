using KingPim.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingPim.WEB.Models
{
    public class ExportViewModel
    {
        public Catalog Catalog { get; set; }
        public List<Category> Category { get; set; }
        public List<Subcategory> Subcategory { get; set; }
        public List<Product> Product { get; set; }

        public int getCatalog { get; set; }
        public int getCategory { get; set; }
        public int getSubcategory { get; set; }
        public int getProduct { get; set; }

        public string FileGuid { get; set; }
    }
}
