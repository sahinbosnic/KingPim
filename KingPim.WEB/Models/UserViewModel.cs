using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingPim.DAL.Models;

namespace KingPim.WEB.Models
{
    public class UserViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
