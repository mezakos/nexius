using FluentValidation;
using Nexius.API.Contracts.Requests;

namespace Nexius.API.Validation
{
    public class TodoItemRequestValidator : AbstractValidator<TodoItemRequest>
    {
        public TodoItemRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
