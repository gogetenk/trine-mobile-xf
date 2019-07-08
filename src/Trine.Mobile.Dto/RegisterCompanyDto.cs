using System;
using System.Collections.Generic;
using System.Text;
using static Trine.Mobile.Dto.CompanyDto;
using static Trine.Mobile.Dto.UserItem;

namespace Trine.Mobile.Dto
{
    public class RegisterCompanyDto
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Siret { get; set; }
        public string Rcs { get; set; }
        public string TvaNumber { get; set; }
        public AddressDto Address { get; set; }
        public CompanyType Type { get; set; }
    }
}
