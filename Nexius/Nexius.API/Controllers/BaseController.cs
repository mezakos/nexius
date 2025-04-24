using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Nexius.API.Controllers
{
    public class BaseController : Controller
    {
        protected virtual IActionResult HandleNonSystemExceptions(Exception exception)
        {
            if (exception is ValidationException validationException)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Validation error",
                    Detail = "One or more validation errors occurred."
                };

                foreach (var error in validationException.Errors)
                {
                    problemDetails.Extensions.Add(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(problemDetails);
            }

            return BadRequest();
        }
    }
}
