namespace Trine.Mobile.Model
{
    public class TokenModel
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public long ExpiresIn { get; set; }
        public string TokenType { get; set; }
    }
}
