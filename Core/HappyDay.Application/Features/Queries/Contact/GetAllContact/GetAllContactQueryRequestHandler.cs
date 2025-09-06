using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Contact.GetAllContact;

public class GetAllContactQueryRequestHandler 
    : IRequestHandler<GetAllContactQueryRequest, GeneralResponse<List<GetAllContactQueryResponse>>>
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;

    public GetAllContactQueryRequestHandler(IContactRepository contactRepository, IMapper mapper)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<List<GetAllContactQueryResponse>>> Handle(GetAllContactQueryRequest request, CancellationToken cancellationToken)
    {
        // Repository’den tüm kayıtları çek
        var contacts = await _contactRepository.GetAllAysnc();

        // DTO’ya mapleme
        var response = _mapper.Map<List<GetAllContactQueryResponse>>(contacts);

        // GeneralResponse ile dön
        return new GeneralResponse<List<GetAllContactQueryResponse>>
        {
            Data = response,
            isSuccess = true,
            Message = "İletişim listesi başarıyla getirildi."
        };
    }
}