using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KingPim.DAL.Models
{
    public class ProductFile
    {
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string GuidName { get; set; }
        public MediaType MediaType { get; set; }
        public bool MainFile { get; set; }
        public bool Published { get; set; }
        public DateTime Created { get; set; }

        //[ForeignKey("SystemAttribute")]
        //public int SystemAttributeId { get; set; }
        //public SystemAttribute SystemAttribute { get; set; }
    }
}
