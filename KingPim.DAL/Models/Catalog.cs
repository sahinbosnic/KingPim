using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KingPim.DAL.Models
{
    public class Catalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Category> Category { get; set; }
        [JsonIgnore]
        public ApiKey ApiKey { get; set; }
    }
}
