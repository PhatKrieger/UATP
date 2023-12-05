using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;
using System.Text;
//using ProductAPIVS.Models;
using System.Security.Claims;
//using Microsoft.EntityFrameworkCore;

namespace Card_Management.Handler
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        // Normally would prefer to put the into database
        // TODO replace with Entity Framework
        private readonly List<User> users = new List<User>();
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> option, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock) : base(option, logger, encoder, clock)
        {
            // i have seen worse passwords, people are bad
            users.Add(new User("adminuser", "admin"));
            users.Add(new User("oldtimer", "password123"));
        }
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("No header found");

            var _haedervalue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(_haedervalue.Parameter != null ? _haedervalue.Parameter : string.Empty);
            string credentials = Encoding.UTF8.GetString(bytes);
            if (!string.IsNullOrEmpty(credentials))
            {
                string[] array = credentials.Split(":");
                string username = array[0];
                string password = array[1];
                var user = users.FirstOrDefault(item => item.Username == username && item.Password == password);
                if (user == null)
                    return AuthenticateResult.Fail("UnAuthorized");

                // Generate Ticket
                var claim = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claim, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            else
            {
                return AuthenticateResult.Fail("UnAuthorized");

            }




            return AuthenticateResult.Fail("not done yet");
        }
    }
}
