using AutoMapper;
using FluentValidation;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Company.DeleteCompany;

public class DeleteCompanyCommandRequestHandler(
    ICompanyRepository repository,
    IMapper mapper,
    IValidator<DeleteCompanyCommandRequest> validator)
    : IRequestHandler<DeleteCompanyCommandRequest, GeneralResponse<DeleteCompanyCommandResponse>>
{
    private readonly ICompanyRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<DeleteCompanyCommandRequest> _validator = validator;

    public async Task<GeneralResponse<DeleteCompanyCommandResponse>> Handle(DeleteCompanyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new GeneralResponse<DeleteCompanyCommandResponse>
            {
                Message = validationResult.Errors.First().ErrorMessage,
            };
        }
    
       await _repository.DeleteAsync(request.Id);
        return new GeneralResponse<DeleteCompanyCommandResponse>()
        {
            Message = Messages.MessageConstants.CompanyDeleted
        };
    }
}