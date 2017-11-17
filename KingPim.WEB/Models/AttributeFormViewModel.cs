using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueType = KingPim.DAL.Models.ValueType;

namespace KingPim.WEB.Models
{
    public class AttributeFormViewModel
    {
        public string NewGroup { get; set; }

        public int? SelectGroup { get; set; }
        public string AttributeName { get; set; }
        public ValueType ValueType { get; set; }
    }
}
