using AutoMapper;
using HappyDay.Application.Features.Commands.ContactMessage.CreateContactMessage;
using HappyDay.Application.Features.Queries.ContactMessage.GetByCompanyContactMessage;
using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Mapping;

public class ContactMessageProfile: Profile
{
    public ContactMessageProfile()
    {
        
        CreateMap<ContactMessage, CreateContactMessageCommanRequest>().ReverseMap();
        CreateMap<ContactMessage, GetByCompanyContactMessageResponse>().ReverseMap();
        CreateMap<ContactMessageWithVenue, GetByCompanyContactMessageResponse>();
    }
}