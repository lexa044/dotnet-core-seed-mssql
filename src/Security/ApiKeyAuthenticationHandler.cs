using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using DNSeed.Services;
using DNSeed.Models.Command;

namespace DNSeed.Security
{
    internal sealed class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string ApiKeyHeaderName = "X-Token";
        private readonly IIdentityService _identityService;
        private readonly IWebWorkContext _workContext;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IIdentityService identityService, IWebWorkContext workContext)
            : base(options, logger, encoder, clock)
        {
            _identityService = identityService;
            _workContext = workContext;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            string refresh_token = apiKeyHeaderValues.FirstOrDefault();
            if (string.IsNullOrEmpty(refresh_token))
            {
                return AuthenticateResult.Fail("Invalid Token");
            }

            LoginResponseModel response = await _identityService.LoadOffTokenAsync(refresh_token);
            if (response == null || response.Id == 0)
            {
                return AuthenticateResult.Fail("Invalid X-Token provided.");
            }

            _workContext.Handle(response);

            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, string.Empty) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
