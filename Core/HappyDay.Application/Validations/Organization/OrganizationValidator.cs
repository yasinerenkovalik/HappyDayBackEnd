using FluentValidation;
using HappyDay.Application.Features.Commands.Organization.CreateOrganization;

namespace HappyDay.Application.Validations.Organization;

public class OrganizationValidator:AbstractValidator<CreateOrganizationCommandRequest>
{
    public OrganizationValidator()
    {
        RuleFor(x => x.Description).NotEmpty();
    }
}