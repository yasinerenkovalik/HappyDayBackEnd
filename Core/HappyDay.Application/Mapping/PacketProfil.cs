using AutoMapper;
using HappyDay.Application.Features.Commands.Package.CreatePackage;
using HappyDay.Application.Features.Commands.Package.DeletePackage;
using HappyDay.Application.Features.Commands.Package.UpdatePackage;
using HappyDay.Application.Features.Queries.Package.GetByCompany;

namespace HappyDay.Application.Mapping;

public class PacketProfil:Profile
{
    public PacketProfil()
    {
        CreateMap<CreatePackageCommandRequest, Package>().ReverseMap();
        CreateMap<UpdatePackageCommandResponse, Package>().ReverseMap();
        CreateMap<UpdatePackageCommandRequest, Package>().ReverseMap();
        CreateMap<DeletePackageCommandResponse, Package>().ReverseMap();
        CreateMap<GetByOrganizastionPackageResponse, Package>().ReverseMap();
    }
}