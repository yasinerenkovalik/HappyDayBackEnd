using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HappyDay.Application.Features.Commands.Company.CreateCompany;

public class CreateCompanyCommandRequest:IRequest<GeneralResponse<CreateCompanyCommandResponse>>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public string Adress { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public IFormFile? CoverPhotoPath { get; set; }
}