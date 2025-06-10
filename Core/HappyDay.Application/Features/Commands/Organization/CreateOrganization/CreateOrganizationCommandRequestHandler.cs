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
    private readonly IValidator <CreateOrganizationCommandRequest> _validator;

    public CreateOrganizationCommandRequestHandler(IMapper mapper, IOrganizationRepository repository, IValidator<CreateOrganizationCommandRequest> validator)
    {
        _mapper = mapper;
        _repository = repository;
        _validator = validator;
    }

    public async Task<GeneralResponse<CreateOrganizationCommandResponse>> Handle(CreateOrganizationCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new GeneralResponse<CreateOrganizationCommandResponse>
            {
                Message = Messages.MessageConstants.InvalidOrganizationData
            };
        }
        var organization = _mapper.Map<Domain.Entities.Organization>(request);
        await _repository.AddAsync(organization);
        return new  GeneralResponse<CreateOrganizationCommandResponse>
        {
            Message = Messages.MessageConstants.OrganizationCreated
        };
    }
}