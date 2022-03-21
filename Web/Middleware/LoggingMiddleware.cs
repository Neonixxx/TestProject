using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Web.Middleware
{
    public class LoggingMiddleware
    {
        private const string StartMessageTemplate = "Starting request HTTP {RequestMethod} {RequestPath}.";

        private const string FinishMessageTemplate =
            "Request HTTP {RequestMethod} {RequestPath} finished in {Elapsed:0.0000} ms. HttpStatusCode {StatusCode}.";

        private static readonly ILogger Log = Serilog.Log.ForContext<LoggingMiddleware>();

        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            Log.Write(
                LogEventLevel.Information,
                StartMessageTemplate,
                httpContext.Request.Method,
                httpContext.Request.Path);
            var sw = Stopwatch.StartNew();
            try
            {
                await _next(httpContext);
                sw.Stop();

                var statusCode = httpContext.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                var log = level == LogEventLevel.Error ? LogForErrorContext(httpContext) : Log;
                log.Write(
                    level,
                    FinishMessageTemplate,
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    sw.Elapsed.TotalMilliseconds,
                    statusCode);
            }
            catch (Exception ex)
            {
                sw.Stop();
                LogForErrorContext(httpContext)
                    .Error(
                        ex,
                        FinishMessageTemplate,
                        httpContext.Request.Method,
                        httpContext.Request.Path,
                        sw.Elapsed.TotalMilliseconds,
                        HttpStatusCode.InternalServerError);

                throw;
            }
        }

        private static ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = Log.ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
            {
                result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));
            }

            return result;
        }
    }
}