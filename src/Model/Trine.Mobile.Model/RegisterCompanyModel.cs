using static Trine.Mobile.Model.CompanyModel;

namespace Trine.Mobile.Model
{
    public class RegisterCompanyModel
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Siret { get; set; }
        public string Rcs { get; set; }
        public string TvaNumber { get; set; }
        public AddressModel Address { get; set; }
        public CompanyType Type { get; set; }
    }
}
