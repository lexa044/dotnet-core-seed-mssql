using Microsoft.AspNetCore.Authentication;

namespace DNSeed.Security
{
    internal sealed class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "api_key";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
