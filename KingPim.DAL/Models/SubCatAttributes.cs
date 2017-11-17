using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KingPim.DAL.Models
{
    public class SubCatAttributes
    {
        public int Id { get; set; }
        [ForeignKey("Subcategory")]
        public int SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }
        [ForeignKey("AttributeGroup")]
        public int AttributeGroupId { get; set; }
        public AttributeGroup AttributeGroup { get; set; }
    }
}
