using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Company.UpdateCompany;

public class UpdateCompanyCommandRequest:IRequest<GeneralResponse<UpdateCompanyCommandResponse>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Adress { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }
}