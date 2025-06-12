using FluentValidation;
using HappyDay.Application.Features.Commands.User.CreateUser;

namespace HappyDay.Application.Validations.UserValidator;

public class UserCreateValidator:AbstractValidator<CreateUserCommandRequest>
{
    public UserCreateValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
    }
}