using KingPim.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingPim.WEB.Models
{
    public class ProductAddViewModel
    {
        public Product Product { get; set; }
        public List<Subcategory> Subcategories { get; set; }
        public List<AttributeGroup> AttributeGroup { get; set; }
        public List<SubCatAttributes> SubCatAttributes { get; set; }

        public int ReturnId { get; set; }
        public int FileId { get; set; }
        public MediaType MediaType { get; set; }
    }
}
