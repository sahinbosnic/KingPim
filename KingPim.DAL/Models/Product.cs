using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KingPim.DAL.Models
{
    public class Product
    {
        public int Id { get; set; }
        [ForeignKey("Subcategory")]
        [JsonIgnore]
        public int SubcategoryId { get; set; }
        [JsonIgnore]
        public Subcategory Subcategory { get; set; }
        public string Name { get; set; }
        public List<AttributeValue> AttributeValue { get; set; }
        [ForeignKey("SystemAttribute")]
        [JsonIgnore]
        public int SystemAttributeId { get; set; }
        public SystemAttribute SystemAttribute { get; set; }
        public bool Published { get; set; }


        public List<ProductFile> ProductFiles { get; set; }
    }
}
