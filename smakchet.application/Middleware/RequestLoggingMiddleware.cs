using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace smakchet.application.Middleware
{
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            
            // Read and log request body
            context.Request.EnableBuffering();
            var requestBody = await ReadBodyFromRequest(context.Request);
            
            // Sanitize sensitive fields (simple example: replace passwords)
            requestBody = SanitizeLogData(requestBody);

            logger.LogInformation(
                "Incoming Request: {Method} {Path} | Body: {Body}",
                context.Request.Method,
                context.Request.Path,
                string.IsNullOrEmpty(requestBody) ? "(empty)" : requestBody);

            // Capture response body
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await next(context);

                sw.Stop();
                
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                logger.LogInformation(
                    "Outgoing Response: {Method} {Path} | Status: {StatusCode} | Time: {Elapsed}ms | Body: {ResponseBody}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    sw.ElapsedMilliseconds,
                    string.IsNullOrEmpty(responseText) ? "(empty)" : responseText);

                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                sw.Stop();
                logger.LogError(
                    ex,
                    "Request Failed: {Method} {Path} | Status: 500 | Time: {Elapsed}ms",
                    context.Request.Method,
                    context.Request.Path,
                    sw.ElapsedMilliseconds);
                    
                // Do not rethrow here, ExceptionMiddleware will handle it and return a standardized response
                throw;
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            request.Body.Position = 0;
            using var streamReader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
            var body = await streamReader.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }

        private string SanitizeLogData(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            
            try
            {
                // Basic check for JSON
                if (input.TrimStart().StartsWith("{") || input.TrimStart().StartsWith("["))
                {
                    using var jsonDoc = JsonDocument.Parse(input);
                    // This is a simplified sanitization. In a real production app, 
                    // you'd use a more robust way to traverse and mask specific keys (e.g., "password", "token")
                    // Here we just check for "password" string for demonstration
                    if (input.Contains("\"password\"", StringComparison.OrdinalIgnoreCase))
                    {
                        return "[SANITIZED - Contains sensitive fields]";
                    }
                }
            }
            catch
            {
                // Not JSON or parse failed, return as is
            }
            
            return input;
        }
    }
}