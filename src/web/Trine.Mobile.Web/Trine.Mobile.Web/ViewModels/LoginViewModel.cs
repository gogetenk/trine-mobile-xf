﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trine.Mobile.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
