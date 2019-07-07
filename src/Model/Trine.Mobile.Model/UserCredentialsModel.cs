namespace Trine.Mobile.Model
{
    public class UserCredentialsModel
    {

        public UserCredentialsModel(string email, string password)
        {
            Mail = email;
            Password = password;
        }

        public string Mail { get; set; }
        public string Password { get; set; }
    }
}
