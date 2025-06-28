using HappyDay.Application.Features.Commands.Organization.CreateOrganization;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HappyDay.Application.Features.Commands.Organization.UpdateOrganization;

public class UpdateOrganizationCommandRequest:IRequest<GeneralResponse<UpdateOrganizationCommandResponse>>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int MaxGuestCount { get; set; }
    public int CategoryId { get; set; }
    public string Location { get; set; }
    public List<string> Services { get; set; } = new();
    public string Duration { get; set; }
    public bool IsOutdoor { get; set; }
    public string ReservationNote { get; set; }
    public string CancelPolicy { get; set; }
    public string VideoUrl { get; set; }
    public IFormFile CoverPhoto { get; set; }
    public List<IFormFile> Images { get; set; }
}