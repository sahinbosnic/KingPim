using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingPim.WEB.Models
{
    public class ResetPasswordViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
