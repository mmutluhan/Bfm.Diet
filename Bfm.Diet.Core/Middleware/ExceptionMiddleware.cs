using System;
using System.Threading.Tasks;
using Bfm.Diet.Core.Converter.JsonConverter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bfm.Diet.Core.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _log;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> log)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _log = log;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
                switch (context.Response.StatusCode)
                {
                    case 404:
                        HandlePageNotFound(context);
                        break;

                    case 500:
                        // TODO 
                        break;
                }
            }

            #region close

            //catch (ApplicationException ex)
            //{
            //    if (context.Response.HasStarted) throw;

            //    context.Response.StatusCode = (int) ex.Status;
            //    context.Response.ContentType = "application/json";
            //    context.Response.Headers.Add("exception", "messageException");
            //    var json = JsonConvert.SerializeObject(new {ex.Message}, _jsonSettings);
            //    _log.LogError(ex, ex.ExceptionMessage, json);
            //    //await context.Response.WriteAsync(json);
            //    context.Response.Redirect($"/system/error/{ context.Response.StatusCode }");
            //} 

            #endregion

            catch (Exception ex)
            {
                HandleException(context, ex);
            }
        }

        private void HandleException(HttpContext context, Exception ex)
        {
            var exception = JsonConvert.SerializeObject(ex, DietJsonSettings.OwnJsonSerializerSettings);
            var cookieOptions = new CookieOptions {Expires = DateTime.Now.AddMilliseconds(10000), IsEssential = true};
            context.Response.Cookies.Append("PageException", exception, cookieOptions);
            context.Response.Cookies.Append("PageExceptionPath", context.Request.Path, cookieOptions);
            _log.LogError(ex, ex.Message, exception);
            context.Response.Redirect("/system/error/500");
        }

        private void HandlePageNotFound(HttpContext context)
        {
            var pageNotFound = context.Request.Path.ToString().TrimStart('/');
            var cookieOptions = new CookieOptions {Expires = DateTime.Now.AddMilliseconds(10000), IsEssential = true};
            context.Response.Cookies.Append("PageNotFound", pageNotFound, cookieOptions);
            context.Response.Redirect("/system/PageNotFound");
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}