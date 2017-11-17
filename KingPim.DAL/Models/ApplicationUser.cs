using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KingPim.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("Catalog")]
        public int? CatalogId { get; set; }
        public Catalog Catalog { get; set; }
        public ApiKey ApiKey { get; set; }
    }
}
