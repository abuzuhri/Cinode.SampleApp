namespace GoTech.Framework.Core.Authentication
{
    public class AuthResponce
    {
        public string access_token { get; set; }
        public string Username { get; set; }
        public string token_type { get; set; } = "Bearer";
    }
}