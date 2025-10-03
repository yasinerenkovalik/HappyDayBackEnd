using FluentValidation;
using HappyDay.Application.Features.Commands.Package.CreatePackage;

namespace HappyDay.Application.Validations.Package;

public class PackageValidator
{
    public class OrganizationValidator:AbstractValidator<CreatePackageCommandRequest>
    {
        public OrganizationValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}