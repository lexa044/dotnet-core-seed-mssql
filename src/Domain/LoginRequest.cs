namespace DNSeed.Domain
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string IPAddress { get; set; }
        public string DeviceId { get; set; }
    }
}
