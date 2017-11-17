using KingPim.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingPim.WEB.Models
{
    public class SettingsViewModel
    {
        public ResetPasswordViewModel Reset { get; set; }
        public ApiKey ApiKey { get; set; }
    }
}
