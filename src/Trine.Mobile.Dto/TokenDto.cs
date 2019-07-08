using System;
using System.Collections.Generic;
using System.Text;

namespace Trine.Mobile.Dto
{
    public class TokenDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public long ExpiresIn { get; set; }
        public string TokenType { get; set; }
    }
}
