using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KingPim.DAL.Models
{
    public class ApiKey
    {
        public int Id { get; set; }
        [ForeignKey("Catalog")]
        public int? CatalogId { get; set; }
        public Catalog Catalog { get; set; }        
        public string Key { get; set; }
    }
}
