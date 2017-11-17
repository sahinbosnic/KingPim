using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KingPim.DAL.Models
{
    public class AttributeGroup
    {
        public int Id { get; set; }
        [ForeignKey("Catalog")]
        public int? CatalogId { get; set; }
        public string Name { get; set; }
        public List<SubCatAttributes> SubCatAttributes { get; set; }
        public List<Attribute> Attribute { get; set; }
    }
}
