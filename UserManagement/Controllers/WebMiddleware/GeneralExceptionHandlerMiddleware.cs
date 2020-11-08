using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserManagement.Domain.Exceptions;

namespace UserManagement.Controllers.WebMiddleware
{
    public class GeneralExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GeneralExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<GeneralExceptionHandlerMiddleware> logger)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                await HandleException(e, logger, context);
            }
        }

        private async Task HandleException(Exception exception, ILogger logger, HttpContext context)
        {
            var traceIdentifier = context.TraceIdentifier;
            var message = exception.Message;
            string? typeOfException = exception.GetType().Name;
            HttpStatusCode httpStatusCode;
            LogLevel logLevel;

            if (exception is ValidationException)
            {
                httpStatusCode = HttpStatusCode.BadRequest;
                logLevel = LogLevel.Information;
            }
            else if (exception is NotFoundException)
            {
                httpStatusCode = HttpStatusCode.NotFound;
                logLevel = LogLevel.Information;
            }
            else if (exception is ConflictException)
            {
                httpStatusCode = HttpStatusCode.Conflict;
                logLevel = LogLevel.Information;
            }
            else
            {
                message = "Unknown error Occurs";
                typeOfException = null;
                httpStatusCode = HttpStatusCode.InternalServerError;
                logLevel = LogLevel.Error;
            }

            logger.Log(logLevel, default, typeof(GeneralExceptionHandlerMiddleware), exception, (type, ex) => $"TraceId: {traceIdentifier}{Environment.NewLine}{exception.Message}");

            var errorHttpResponse = new ErrorHttpResponse(message, typeOfException);
            var errorHttpContentStr = JsonConvert.SerializeObject(errorHttpResponse);

            context.Response.StatusCode = (int) httpStatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(errorHttpContentStr);
        }

        private class ErrorHttpResponse
        {
            public ErrorHttpResponse(string friendlyMessage, string? exceptionType = null)
            {
                FriendlyMessage = friendlyMessage ?? throw new ArgumentNullException(nameof(friendlyMessage));
                ExceptionType = exceptionType;
            }

            public string FriendlyMessage { get; }
            public string? ExceptionType { get; }
        }
    }
}