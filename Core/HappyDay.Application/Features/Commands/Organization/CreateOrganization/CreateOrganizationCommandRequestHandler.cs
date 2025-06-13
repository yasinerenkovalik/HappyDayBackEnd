using AutoMapper;
using FluentValidation;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Organization.CreateOrganization;

public class CreateOrganizationCommandRequestHandler:IRequestHandler<CreateOrganizationCommandRequest, GeneralResponse<CreateOrganizationCommandResponse>>
{
    private readonly IMapper _mapper;
    private readonly IOrganizationRepository  _repository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IValidator <CreateOrganizationCommandRequest> _validator;

    public CreateOrganizationCommandRequestHandler(IMapper mapper, IOrganizationRepository repository, IValidator<CreateOrganizationCommandRequest> validator, ICompanyRepository companyRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _validator = validator;
        _companyRepository = companyRepository;
    }

    public async Task<GeneralResponse<CreateOrganizationCommandResponse>> Handle(CreateOrganizationCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new GeneralResponse<CreateOrganizationCommandResponse>
            {
                Message = Messages.MessageConstants.InvalidOrganizationData,
                isSuccess = false
                
            };
        }
   
        var company = await _companyRepository.GetByIdAsync(request.CompanyId);
        if (company == null)
        {
            return new GeneralResponse<CreateOrganizationCommandResponse>
            {
                Message = Messages.MessageConstants.CompanyNotFound,
                isSuccess = false
            };
        }
        var organization = _mapper.Map<Domain.Entities.Organization>(request);
        await _repository.AddAsync(organization);
        return new  GeneralResponse<CreateOrganizationCommandResponse>
        {
            Message = Messages.MessageConstants.OrganizationCreated,
            isSuccess = true
        };
    }
}