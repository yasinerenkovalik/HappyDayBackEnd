using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using HappyDay.Domain.Entities;
using MediatR;

namespace HappyDay.Application.Features.Commands.Contact.CreateContact;




public class CreateContactCommandRequestHandler 
    : IRequestHandler<CreateContactCommandRequest, GeneralResponse<CreateContactCommandResponse>>
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;

    public CreateContactCommandRequestHandler(IContactRepository contactRepository, IMapper mapper)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<CreateContactCommandResponse>> Handle(CreateContactCommandRequest request, CancellationToken cancellationToken)
    {
        // Request'i entity'ye mapliyoruz
        var contactEntity = _mapper.Map<Domain.Entities.Contact>(request);

        // Veritabanına kaydet
        await _contactRepository.AddAsync(contactEntity);
   

        // Response için tekrar mapleme
     

        return new GeneralResponse<CreateContactCommandResponse>()
        {
            isSuccess = true
        };
    }
}