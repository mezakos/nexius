using FluentValidation;
using Nexius.API.Contracts.Responses;
using System.Net;

namespace Nexius.API.Validation
{
    public static class Extensions
    {
        public static ErrorResponse ToErrorResponse(this ValidationException validationException)
        {
            var result = new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorDetails = validationException.Errors.GroupBy(x => x.PropertyName)
                                .ToDictionary(
                                    g => g.Key,
                                    g => g.Select(x => x.ErrorMessage).ToArray())
            };

            return result;
        }
    }
}
