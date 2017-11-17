using KingPim.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueType = KingPim.DAL.Models.ValueType;

namespace KingPim.WEB.Models
{
    public class AttributeViewModel
    {
        public List<AttributeGroup> AttributeGroup { get; set; }
        public AttributeFormViewModel Form { get; set; }
    }
}
