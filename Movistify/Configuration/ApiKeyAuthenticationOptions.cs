using Microsoft.AspNetCore.Authentication;

namespace Movistify.Configuration
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string ApiKey { get; set; }
    }
}