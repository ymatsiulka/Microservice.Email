using ArchitectProg.Kernel.Extensions.Exceptions;
using ArchitectProg.Kernel.Extensions.Utils;
using ArchitectProg.WebApi.Extensions.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Email.Extensions;

public static class ResultExtensions
{
    public static IActionResult MatchActionResult<T>(
        this Result<T> result,
        Func<T?, IActionResult> actionResult)
    {
        var response = result.Match(actionResult, x => MatchException(x));
        return response;
    }

    public static IActionResult MatchActionResult(this Result result, Func<IActionResult> actionResult)
    {
        var response = result.Match(actionResult, x => MatchException(x));
        return response;
    }

    private static IActionResult MatchException(Exception? exception)
    {
        switch (exception)
        {
            case ResourceNotFoundException:
                var notFoundError = new ErrorResult(StatusCodes.Status404NotFound, exception.Message);
                return new NotFoundObjectResult(notFoundError);
            case ValidationException:
                var badRequestError = new ErrorResult(StatusCodes.Status400BadRequest, exception.Message);
                return new BadRequestObjectResult(badRequestError);
            case UnauthorizedException:
                var unauthorizedError = new ErrorResult(StatusCodes.Status401Unauthorized, exception.Message);
                return new UnauthorizedObjectResult(unauthorizedError);
            default:
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}