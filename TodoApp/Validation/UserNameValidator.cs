﻿using FluentValidation;
using TodoApp.Dtos;
using TodoApp.Models;

namespace TodoApp.Validation
{
    public class UserNameValidator :AbstractValidator<RegisterDto>
    {
        public UserNameValidator()
        {
            RuleFor(x => x.Username).NotEmpty().
                WithMessage("Username con not be empty")
                .MinimumLength(5).
                WithMessage("Username contains at least 5 characters");
                
        }
    }
}
