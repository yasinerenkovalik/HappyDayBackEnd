using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HappyDay.Application.Features.Commands.Organization.CreateOrganization;

public class CreateOrganizationCommandRequest:IRequest<GeneralResponse<CreateOrganizationCommandResponse>>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int MaxGuestCount { get; set; }
    public IFormFile CoverPhoto { get; set; }
    public Guid CompanyId { get; set; }

    public List<IFormFile> Images { get; set; }
}