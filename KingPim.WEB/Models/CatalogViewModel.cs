using KingPim.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingPim.WEB.Models
{
    public class CatalogViewModel
    {
        public List<Category> Categories { get; set; }
        public List<AttributeGroup> AttributeGroups { get; set; }
        public Catalog Catalog { get; set; }
        public CatalogFormViewModel Form { get; set; }
    }
}
