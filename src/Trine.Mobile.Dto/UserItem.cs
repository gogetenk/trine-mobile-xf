namespace Trine.Mobile.Dto
{
    public class UserItem
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public AddressDto Address { get; set; }
        public string Siret { get; set; }
        public string SignatureFileUrl { get; set; }
        public string BankDetailsFileUrl { get; set; }
        public string LegalContributionFileUrl { get; set; }
        public UserTypeEnum UserType { get; set; }

        public enum UserTypeEnum
        {
            COMMERCIAL,
            CUSTOMER,
            CONSULTANT
        }
    }
}