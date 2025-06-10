using FluentValidation;
using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Features.Commands.Company.DeleteCompany;

namespace HappyDay.Application.Validations.Company;


    public class CompanyCreateValidator : AbstractValidator<CreateCompanyCommandRequest>
    {
        public CompanyCreateValidator()
        {
            RuleFor(x => x.Adress).NotEmpty();
        }
    }
    public class CompanyDeleteValidator : AbstractValidator<DeleteCompanyCommandRequest>
    {
        public CompanyDeleteValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class CompanyUpdateValidator : AbstractValidator<DeleteCompanyCommandRequest>
    {
        public CompanyUpdateValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

