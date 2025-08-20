using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Organization.DeleteOrganization;

public class DeleteOrganizationCommandRequest:IRequest<GeneralResponse<DeleteOrganizationCommandResponse>>
{
    public Guid Id { get; set; }
}