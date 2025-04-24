using FluentValidation;
using Nexius.API.Contracts.Requests;

namespace Nexius.API.Validation
{
    public class EditTodoItemRequestValidator : AbstractValidator<EditTodoItemRequest>
    {
        public EditTodoItemRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().NotEqual(Guid.Empty);
            RuleFor(x => x.Description).Must(x => x == null || !string.IsNullOrWhiteSpace(x));
        }
    }
}
