namespace DNSeed.Models.Command
{
    public class LoginRequestModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string GrantType { get; set; }
    }
}
