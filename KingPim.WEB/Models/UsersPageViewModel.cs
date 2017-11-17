using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingPim.WEB.Models
{
    public class UsersPageViewModel
    {
        public List<UserViewModel> Users { get; set; }
        public UserFormViewModel Form { get; set; }
    }
}
