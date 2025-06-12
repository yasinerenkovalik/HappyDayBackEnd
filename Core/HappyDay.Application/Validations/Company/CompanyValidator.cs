using FluentValidation;
using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Features.Commands.Company.DeleteCompany;
using HappyDay.Application.Features.Commands.Company.UpdateCompany;

namespace HappyDay.Application.Validations.Company;

public class CompanyCreateValidator : AbstractValidator<CreateCompanyCommandRequest>
{
    public CompanyCreateValidator()
    {
        RuleFor(x => x.Adress).NotEmpty().WithMessage(Messages.MessageConstants.CompanyAdressNotEmty);
        RuleFor(x => x.PhoneNumber).MaximumLength(12).WithMessage("en az 12 karakter girilmei");
        RuleFor(x => x.PhoneNumber).MinimumLength(12).WithMessage("");
    }
}

public class CompanyDeleteValidator : AbstractValidator<DeleteCompanyCommandRequest>
{
    public CompanyDeleteValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class CompanyUpdateValidator : AbstractValidator<UpdateCompanyCommandRequest>
{
    public CompanyUpdateValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
 
    }
}