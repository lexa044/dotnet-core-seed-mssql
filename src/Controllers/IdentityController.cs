using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using log4net;

using DNSeed.Services;
using DNSeed.Models.Command;
using DNSeed.Domain;

namespace DNSeed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ILog _logger;

        public IdentityController(ILog logger, IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        [ProducesResponseType(typeof(LoginResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] LoginRequestModel request)
        {
            LoginResponseModel response = new LoginResponseModel();
            try
            {
                //TODO: assuming grant_type == "Password", tailor to your needs
                LoginRequest domainRequest = new LoginRequest();
                domainRequest.Username = request.Username;
                domainRequest.Password = request.Password;
                domainRequest.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                domainRequest.DeviceId = Request.Headers?.FirstOrDefault(s => s.Key.ToLower() == "user-agent").Value;
                response = await _identityService.SigninAsync(domainRequest);
            }
            catch (Exception ex)
            {
                _logger.Error("There was an error on 'Token' invocation.", ex);
            }

            return new ObjectResult(response);
        }
    }
}
