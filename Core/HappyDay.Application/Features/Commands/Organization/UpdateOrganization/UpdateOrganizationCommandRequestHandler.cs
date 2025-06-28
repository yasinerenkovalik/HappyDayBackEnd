using AutoMapper;
using HappyDay.Application.Features.Commands.Organization.CreateOrganization;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Organization.UpdateOrganization;

public class UpdateOrganizationCommandRequestHandler:IRequestHandler<UpdateOrganizationCommandRequest, GeneralResponse<UpdateOrganizationCommandResponse>>
{


    private readonly IOrganizationRepository _repository;
    private readonly IMapper _mapper;

    public UpdateOrganizationCommandRequestHandler(IOrganizationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<UpdateOrganizationCommandResponse>> Handle(UpdateOrganizationCommandRequest request, CancellationToken cancellationToken)
    {
        var organization = await _repository.GetByIdAsync(request.Id);
        if (organization == null)
        {
            return new GeneralResponse<UpdateOrganizationCommandResponse>
            {
                Message = Messages.MessageConstants.InvalidOrganizationData,
                isSuccess = false
            };
        }

        // AutoMapper ile request'ten gelen alanları entity'e aktar
        _mapper.Map(request, organization);

        await _repository.UpdateAsync(organization);

        return new GeneralResponse<UpdateOrganizationCommandResponse>
        {
            Message = Messages.MessageConstants.OrganizationUpdated,
            isSuccess = true,
          
        };
    }

}