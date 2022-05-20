using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog;
using System;
using System.Net.Http.Headers;
using XProject.Core;

namespace XProject.WebApi.Filters.Validation
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly ILogger _logger;
        public string User { get; set; }
        public string Pass { get; set; }

        private const string _AuthenticationScheme = "Basic";

        public HangfireAuthorizationFilter() : this(Log.Logger)
        {
        }

        public HangfireAuthorizationFilter(ILogger logger)
        {
            _logger = logger;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var header = httpContext.Request.Headers["Authorization"];

            if (Missing_Authorization_Header(header))
            {
                _logger.Information("Request is missing Authorization Header");
                SetChallengeResponse(httpContext);
                return false;
            }

            var authValues = AuthenticationHeaderValue.Parse(header);

            if (Not_Basic_Authentication(authValues))
            {
                _logger.Information("Request is NOT BASIC authentication");
                SetChallengeResponse(httpContext);
                return false;
            }

            var tokens = Extract_Authentication_Tokens(authValues);

            if (tokens.Are_Invalid())
            {
                _logger.Information("Authentication tokens are invalid (empty, null, whitespace)");
                SetChallengeResponse(httpContext);
                return false;
            }

            if (tokens.Credentials_Match(User, Pass))
            {
                _logger.Information("Awesome, authentication tokens match configuration!");
                return true;
            }

            _logger.Information($"Boo! Authentication tokens [{tokens.Username}] [{tokens.Password}] do not match configuration");

            SetChallengeResponse(httpContext);
            return false;
        }

        private static bool Missing_Authorization_Header(StringValues header)
        {
            return string.IsNullOrWhiteSpace(header);
        }

        private static BasicAuthenticationTokens Extract_Authentication_Tokens(AuthenticationHeaderValue authValues)
        {
            var parameter = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
            var parts = parameter.Split(':');
            return new BasicAuthenticationTokens(parts);
        }

        private static bool Not_Basic_Authentication(AuthenticationHeaderValue authValues)
        {
            return !_AuthenticationScheme.Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase);
        }

        private void SetChallengeResponse(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 401;
            httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
            httpContext.Response.WriteAsync("Authentication is required.");
        }
    }
}
