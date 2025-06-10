using AutoMapper;
using FluentValidation;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Company.CreateCompany;

public class CreateCompanyCommandRequestHandler(
    ICompanyRepository repository,
    IMapper mapper,
    IValidator<CreateCompanyCommandRequest> validator)
    : IRequestHandler<CreateCompanyCommandRequest, GeneralResponse<CreateCompanyCommandResponse>>
{
    public async Task<GeneralResponse<CreateCompanyCommandResponse>> Handle(CreateCompanyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new GeneralResponse<CreateCompanyCommandResponse>
            {
                Message = Messages.MessageConstants.InvalidCompanyData
            };
        }
        var company = mapper.Map<Domain.Entities.Company>(request);
        await repository.AddAsync(company);
        return new  GeneralResponse<CreateCompanyCommandResponse>
        {
            Message = Messages.MessageConstants.CompanyCreated
        };
    }
}