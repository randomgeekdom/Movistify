using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Movistify.Configuration
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly string apiKey;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            this.apiKey = configuration[Constants.ApiKey];
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKey = Request.Headers[$"x{Constants.ApiKey.ToLower()}"];

            if (string.IsNullOrEmpty(apiKey) || apiKey != this.apiKey)
            {
                return Task.FromResult(AuthenticateResult.Fail(Constants.InvalidApiKey));
            }

            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, apiKey) }, Constants.ApiKey);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Constants.ApiKey);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}