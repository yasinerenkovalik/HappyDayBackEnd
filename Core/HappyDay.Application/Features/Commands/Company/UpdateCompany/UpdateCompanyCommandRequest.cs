using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HappyDay.Application.Features.Commands.Company.UpdateCompany;

public class UpdateCompanyCommandRequest:IRequest<GeneralResponse<UpdateCompanyCommandResponse>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Adress { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public IFormFile? CoverPhoto { get; set; }
}