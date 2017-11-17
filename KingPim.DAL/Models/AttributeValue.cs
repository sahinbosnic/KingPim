using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
//using Attribute = KingPim.DAL.Models.Attribute;

namespace KingPim.DAL.Models
{
    public class AttributeValue
    {
        public int Id { get; set; }
        [ForeignKey("Product")]
        [JsonIgnore]
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        [ForeignKey("Attribute")]
        public int AttributeId { get; set; }
        public Attribute Attribute { get; set; }
        [ForeignKey("AttributeGroup")]
        [JsonIgnore]
        public int? AttributeGroupId { get; set; }
        [JsonIgnore]
        public AttributeGroup AttributeGroup { get; set; }
        public string Value { get; set; }
    }
}
