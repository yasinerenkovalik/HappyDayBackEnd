using FluentValidation;

namespace HappyDay.Application.Features.Invitations.RegisterByInvite;

public class RegisterByInviteValidator : AbstractValidator<RegisterByInviteCommand>
{
    public RegisterByInviteValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.CompanyName).NotEmpty();
    }
}