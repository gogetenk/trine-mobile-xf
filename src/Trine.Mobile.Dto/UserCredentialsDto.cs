using System;
using System.Collections.Generic;
using System.Text;

namespace Trine.Mobile.Dto
{
    public class UserCredentialsDto
    {
        public UserCredentialsDto(string email, string password)
        {
            Mail = email;
            Password = password;
        }

        public string Mail { get; set; }
        public string Password { get; set; }
    }
}
