using APIPCOS_CRM.Models;
using APIPCOS_CRM.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Newtonsoft.Json;

namespace APIPCOS_CRM.Helper
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly UserRepository _user;
        //private readonly HttpContextAccessor _httpContextAccessor;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            UserRepository user
            )
            : base(options, logger, encoder, clock)
        {
            _user = user;
            //_httpContextAccessor = httpContextAccessor;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = Context.Request.Cookies["Authorization"];

            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            UserModel user = null;
            if (Context.Request.Headers.ContainsKey("Authorization") || (token != null))
            {
                string[] authHeader, credentials;
                string username, password;
                byte[] credentialBytes;

                try
                {
                    authHeader = (token != null) ? token.ToString().Split(new[] { ' ' }, 2) : Request.Headers["Authorization"].ToString().Split(new[] { ' ' }, 2);
                    credentialBytes = Convert.FromBase64String(authHeader[1]);
                    credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                    username = credentials[0];
                    password = credentials[1];
                    user = _user.Authenticate(username, password);
                }
                catch
                {
                    string result;
                    Context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    result = JsonConvert.SerializeObject(new { error = "Invalid Authorization Header" });
                    Context.Response.ContentType = "application/json";
                    //await Context.Response.WriteAsync(result);
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
                }
            }
            else
            {
                string result;
                Context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                result = JsonConvert.SerializeObject(new { error = "Missing Authorization Header" });
                Context.Response.ContentType = "application/json";
                // await Context.Response.WriteAsync(result);
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
            }

            if (user == null)
            {
                string result;
                Context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                result = JsonConvert.SerializeObject(new { error = "Invalid Username or Password" });
                Context.Response.ContentType = "application/json";
                // await Context.Response.WriteAsync(result);
                return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
            }

            var claims = new[] {
                //new Claim(ClaimTypes.Role, user.IDRole.ToString()),
                new Claim(ClaimTypes.Name, user.username),
                
               /// new Claim(ClaimTypes.Sid , $"{user.id}" ),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        /* protected override Task HandleChallengeAsync(AuthenticationProperties properties)
         {
             if (Request.Path.StartsWithSegments("/api"))
             {
                 // Nếu yêu cầu đến từ API, trả về lỗi 401 Unauthorized
                 Response.StatusCode = 401;
                 return Task.CompletedTask;
             }
             else
             {
                 // Nếu yêu cầu đến từ các đường dẫn khác, thực hiện chuyển hướng đến trang đăng nhập
                 Response.Redirect("/auth/login");
                 return Task.CompletedTask;
             }
         }*/
    }
}
