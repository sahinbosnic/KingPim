using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
//using Attribute = KingPim.DAL.Models.Attribute;

namespace KingPim.DAL.Models
{
    public class Attribute
    {
        public int Id { get; set; }
        [ForeignKey("AttributeGroup")]
        [JsonIgnore]
        public int? AttributeGroupId { get; set; }
        [JsonIgnore]
        public AttributeGroup AttributeGroup { get; set; }
        [JsonIgnore]
        public List<AttributeValue> AttributeValue { get; set; }
        public string Name { get; set; }
        public ValueType ValueType { get; set; }
    }
}
