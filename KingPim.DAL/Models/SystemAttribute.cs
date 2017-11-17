using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KingPim.DAL.Models
{
    public class SystemAttribute
    {
        public int Id { get; set; }
        public DateTime LastModified { get; set; }
        [JsonIgnore]
        public DateTime Created { get; set; }
        [JsonIgnore]
        public string ModifiedBy { get; set; }
        /*[ForeignKey("ApplicationUser")]
        public string ModifiedBy { get; set; }*/
        //public List<SubCatAttributes> SubCatAttributes { get; set; }
        [JsonIgnore]
        public bool Published { get; set; }
        [JsonIgnore]
        public int Version { get; set; }
    }
}
