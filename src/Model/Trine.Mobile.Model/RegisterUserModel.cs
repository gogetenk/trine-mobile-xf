using static Trine.Mobile.Model.UserModel;

namespace Trine.Mobile.Model
{
    public class RegisterUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatedPassword { get; set; }
        public string InvitationCode { get; set; }

        // Utilisé pour les tests de Chris (ignorer cette prop)
        public GlobalRoleEnum? GlobalRole { get; set; }

        public enum GlobalRoleEnum
        {
            Customer,
            Consultant,
            Admin
        }
    }
}
