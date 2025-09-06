using AutoMapper;
using HappyDay.Application.Features.Commands.Contact.CreateContact;
using HappyDay.Application.Features.Queries.Contact.GetAllContact;
using HappyDay.Application.Features.Queries.Contact.GetByIdContact;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Mapping;

public class ContactProfile: Profile
{
    public ContactProfile()
    {
        CreateMap<Contact, CreateContactCommandRequest>().ReverseMap();
        CreateMap<Contact, GetAllContactQueryResponse>().ReverseMap();
        CreateMap<Contact, GetByIdContactQueryResponse>().ReverseMap();
    }
}