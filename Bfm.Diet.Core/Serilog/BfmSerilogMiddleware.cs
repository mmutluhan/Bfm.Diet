using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bfm.Diet.Core.Security.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace Bfm.Diet.Core.Serilog
{
    public class BfmSerilogMiddleware : IMiddleware
    {
        private readonly BfmSerilogOptions _options;

        public BfmSerilogMiddleware(IOptions<BfmSerilogOptions> options)
        {
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var enrichers = new List<ILogEventEnricher>();
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                var jwtEncodedString = authHeader.Substring(7);
                if (!string.IsNullOrEmpty(jwtEncodedString))
                {
                    var claims = TokenHelper.DecodeToken(jwtEncodedString);
                    if (claims != null && claims.Any())
                    {
                        var sid = claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
                        var name = claims.FirstOrDefault(x => x.Type == "unique_name");
                        var email = claims.FirstOrDefault(x => x.Type == "email");
                        var exp = claims.FirstOrDefault(x => x.Type == "exp");
                        var aud = claims.FirstOrDefault(x => x.Type == "aud");
                        var iss = claims.FirstOrDefault(x => x.Type == "iss");

                        if (sid != null)
                            enrichers.Add(new PropertyEnricher(_options.EnricherPropertyNames.UserId, sid.Value));

                        if (name != null)
                            enrichers.Add(new PropertyEnricher(_options.EnricherPropertyNames.Adi, name.Value));

                        if (email != null)
                            enrichers.Add(new PropertyEnricher(_options.EnricherPropertyNames.EMail, email.Value));

                        enrichers.Add(new PropertyEnricher(_options.EnricherPropertyNames.Token, jwtEncodedString));

                        if (exp != null)
                            enrichers.Add(new PropertyEnricher(_options.EnricherPropertyNames.Expiration, exp.Value));
                        if (iss != null)
                            enrichers.Add(new PropertyEnricher(_options.EnricherPropertyNames.Issuer, iss.Value));

                        if (aud != null)
                            enrichers.Add(new PropertyEnricher(_options.EnricherPropertyNames.Audience, aud.Value));
                    }
                }
            }

            using (LogContext.Push(enrichers.ToArray()))
            {
                await next(context);
            }
        }
    }
}