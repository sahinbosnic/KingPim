using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KingPim.WEB.Models
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("E-mail")]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Missing password.")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }
    }
}
