using AutoMapper;
using HappyDay.Application.Features.Commands.ContactMessage.CreateContactMessage;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Mapping;

public class ContactMessageProfile: Profile
{
    public ContactMessageProfile()
    {
        CreateMap<ContactMessage, CreateContactMessageCommanRequest>().ReverseMap();
    }
}