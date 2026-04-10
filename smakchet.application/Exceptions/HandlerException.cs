using Microsoft.AspNetCore.Http;
using smakchet.application.DTOs.Error;
using smakchet.application.DTOs.Success;
using System.Text.Json;

namespace smakchet.application.Exceptions;

public static class HandlerException
{
    public static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        ErrorDto error;
        int statusCode;
        string message;

        switch (exception)
        {
            case NotFoundException nfEx:
                statusCode = StatusCodes.Status404NotFound;
                message = nfEx.Message;
                error = new ErrorDto
                {
                    Code = nfEx.ErrorCode,
                    Message = nfEx.Message
                };
                break;

            case BadRequestException brEx:
                statusCode = StatusCodes.Status400BadRequest;
                message = brEx.Message;
                error = new ErrorDto
                {
                    Code = brEx.ErrorCode,
                    Message = brEx.Message
                };
                break;

            case DuplicateException dEx:
                statusCode = StatusCodes.Status409Conflict;
                message = dEx.Message;
                error = new ErrorDto
                {
                    Code = dEx.ErrorCode,
                    Message = dEx.Message
                };
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                message = "An unexpected error occurred.";
                error = new ErrorDto
                {
                    Code = "InternalServerError",
                    Message = message
                };
                break;
        }

        context.Response.StatusCode = statusCode;
        var response = ResponseDto<object>.Fail(message, error);
        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}