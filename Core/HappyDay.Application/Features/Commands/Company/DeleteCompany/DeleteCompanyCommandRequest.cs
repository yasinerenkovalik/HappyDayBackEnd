using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Company.DeleteCompany;

public class DeleteCompanyCommandRequest:IRequest<GeneralResponse<DeleteCompanyCommandResponse>>
{
    public Guid Id { get; set; }
}