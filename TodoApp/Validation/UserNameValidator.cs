using FluentValidation;
using TodoApp.Dtos;
using TodoApp.Models;

namespace TodoApp.Validation
{
    public class UserNameValidator :AbstractValidator<RegisterDto>
    {
        public UserNameValidator()
        {
            RuleFor(x => x.Username).NotEmpty()
                .MinimumLength(5);
        }
    }
}
