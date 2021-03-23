using System;

namespace DNSeed.Domain
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public DateTime LastLogin { get; set; }
        public string LastIPAddress { get; set; }
        public string LastDeviceId { get; set; }
        public string Token { get; set; }
    }
}
