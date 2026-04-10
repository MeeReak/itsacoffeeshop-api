using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using smakchet.application.DTOs.Error;
using smakchet.application.DTOs.Success;
using System.Text.RegularExpressions;

namespace smakchet.api.Extensions;

public static class InvalidModelStateResponse
{
    public static IActionResult ProduceErrorResponse(ActionContext context)
    {
        var errors = context.ModelState
            .Where(ms => ms.Value?.ValidationState == ModelValidationState.Invalid)
            .SelectMany(kvp => kvp.Value!.Errors.Select(e =>
            {
                var field = NormalizeField(kvp.Key);
                var message = NormalizeMessage(e.ErrorMessage, field);

                return new ErrorDetailDto
                {
                    Code = "ValidationError",
                    Target = field,
                    Message = message
                };
            }))
            .ToList();

        var error = new ErrorDto
        {
            Code = "InvalidModelState",
            Message = "Validation failed for one or more fields.",
            Details = errors
        };

        var response = ResponseDto<object>.Fail("Validation failed", error);

        return new BadRequestObjectResult(response);
    }

    private static string NormalizeField(string field)
    {
        field = field.Replace("$.", "");

        field = Regex.Replace(field, @"\[\d+\]", "");

        var parts = field.Split('.');
        field = string.Join(".", parts.Select(p =>
            char.ToLowerInvariant(p[0]) + p.Substring(1)));

        return field;
    }

    private static string NormalizeMessage(string message, string field)
    {
        if (string.IsNullOrWhiteSpace(message))
            return $"Invalid value for '{field}'.";

        if (message.Contains("could not be converted"))
            return $"Invalid value for '{field}'.";

        return message;
    }
}