using System;
using System.Collections.Generic;
using System.Text;

namespace Trine.Mobile.Dto
{
    public class PasswordUpdateDto
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
