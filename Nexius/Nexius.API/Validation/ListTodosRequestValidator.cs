using FluentValidation;
using Nexius.API.Contracts.Requests;

namespace Nexius.API.Validation
{
    public class ListTodosRequestValidator : AbstractValidator<ListTodosRequest>
    {
        public ListTodosRequestValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(1);
        }
    }
}
