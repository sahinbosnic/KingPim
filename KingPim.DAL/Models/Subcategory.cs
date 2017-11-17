using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KingPim.DAL.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        [JsonIgnore][ForeignKey("Category")]
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<SubCatAttributes> SubCatAttributes { get; set; }
        public List<Product> Product { get; set; }
        public bool Published { get; set; }
    }
}
