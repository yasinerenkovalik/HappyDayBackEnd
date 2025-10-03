using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Package.CreatePackage;

public class CreatePackageCommandRequest:IRequest<GeneralResponse<CreatePackageCommandResponse>>
{
    public Guid OrganizationId { get; set; }   // Hangi organizasyona bağlı
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal? PricePerPerson { get; set; }
    public int? MaxGuests { get; set; }
}