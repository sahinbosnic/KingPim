using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace KingPim.DAL.Models
{
    public class Category
    {
        public int Id { get; set; }
        [JsonIgnore][ForeignKey("Catalog")]
        public int CatalogId { get; set; }
        [JsonIgnore]
        public Catalog Catalog { get; set; }
        public string Name { get; set; }
        public List<Subcategory> Subcategory { get; set; }
        public bool Published { get; set; }
    }
}
