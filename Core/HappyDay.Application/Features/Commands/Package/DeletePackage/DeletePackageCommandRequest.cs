using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Package.DeletePackage;

public class DeletePackageCommandRequest:IRequest<GeneralResponse<DeletePackageCommandResponse>>
{
    public Guid Id { get; set; }
}