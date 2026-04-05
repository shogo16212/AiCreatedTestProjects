using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using ProductManagement_Api_20260404.Models;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace ProductManagement_Api_20260404
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private DB db = new DB();
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var header = Request.Headers.Authorization;
                if (string.IsNullOrEmpty(header)) return Task.FromResult(AuthenticateResult.NoResult());

                var value = AuthenticationHeaderValue.Parse(header);
                var bytes = Convert.FromBase64String(value.Parameter);
                var creds = Encoding.UTF8.GetString(bytes).Split(":");

                var username = creds[0];
                var password = creds[1];
                var user = db.Products.ToList().FirstOrDefault(a => a.ProductName == username && a.ProductName == password);
                if (user == null) return Task.FromResult(AuthenticateResult.NoResult());

                var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, user.ProductId.ToString()) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));


            }
            catch (Exception)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            return Response.WriteAsJsonAsync(new { Error = "Unauthorized" });
        }
    }
}
