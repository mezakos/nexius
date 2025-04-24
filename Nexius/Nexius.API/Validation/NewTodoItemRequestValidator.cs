using FluentValidation;
using Nexius.API.Contracts.Requests;

namespace Nexius.API.Validation
{
    public class NewTodoItemRequestValidator : AbstractValidator<NewTodoItemRequest>
    {
        public NewTodoItemRequestValidator()
        {
            RuleFor(x => x.Description).NotNull().NotEmpty();
        }
    }
}
