using FluentValidation;
using HappyDay.Application.Features.Commands.Organization.CreateOrganization;
using HappyDay.Application.Features.Commands.Organization.DeleteOrganization;

namespace HappyDay.Application.Validations.Organization;

public class OrganizationValidator:AbstractValidator<CreateOrganizationCommandRequest>
{
    public OrganizationValidator()
    {
        RuleFor(x => x.Description).NotEmpty();
    }
}
public class DeleteOrganizationValidator : AbstractValidator<DeleteOrganizationCommandRequest>
{
    public DeleteOrganizationValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Organizasyon ID boş olamaz.");
    }
}