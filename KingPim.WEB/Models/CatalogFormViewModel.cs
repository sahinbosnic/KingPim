using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingPim.WEB.Models
{
    public class CatalogFormViewModel
    {
        public int CatalogId { get; set; }
        public string CatalogName { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool CategoryPublished { get; set; }

        public string SubcategoryName { get; set; }
        public bool SubcategoryPublished { get; set; }
        public List<int> GroupId { get; set; }
    }
}
