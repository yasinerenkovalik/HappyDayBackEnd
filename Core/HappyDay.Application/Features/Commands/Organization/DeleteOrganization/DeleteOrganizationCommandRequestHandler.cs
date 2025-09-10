using AutoMapper;
using FluentValidation;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Organization.DeleteOrganization;

public class DeleteOrganizationCommandRequestHandler:IRequestHandler<DeleteOrganizationCommandRequest, GeneralResponse<DeleteOrganizationCommandResponse>>
{
    private readonly IOrganizationRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<DeleteOrganizationCommandRequest> _validator;

    public DeleteOrganizationCommandRequestHandler(IOrganizationRepository repository, IMapper mapper, IValidator<DeleteOrganizationCommandRequest> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<GeneralResponse<DeleteOrganizationCommandResponse>> Handle(DeleteOrganizationCommandRequest request, CancellationToken cancellationToken)
    {
       await _repository.DeleteAsync(request.Id);
        return new GeneralResponse<DeleteOrganizationCommandResponse>
        {
            
            isSuccess = true
        };
    }
}