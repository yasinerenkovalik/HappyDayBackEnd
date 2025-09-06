using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Contact.GetByIdContact;

public class GetByIdContactQueryRequestHandler 
    : IRequestHandler<GetByIdContactQueryRequest, GeneralResponse<GetByIdContactQueryResponse>>
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;

    public GetByIdContactQueryRequestHandler(IContactRepository contactRepository, IMapper mapper)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<GetByIdContactQueryResponse>> Handle(GetByIdContactQueryRequest request, CancellationToken cancellationToken)
    {
        // İlgili kaydı getir
        var contact = await _contactRepository.GetByIdAsync(request.Id);

        if (contact == null)
        {
            return new GeneralResponse<GetByIdContactQueryResponse>
            {
                Data = null,
                isSuccess = false,
                Message = "İletişim kaydı bulunamadı."
            };
        }

        // Mapleme
        var response = _mapper.Map<GetByIdContactQueryResponse>(contact);

        return new GeneralResponse<GetByIdContactQueryResponse>
        {
            Data = response,
            isSuccess = true,
            Message = "İletişim kaydı başarıyla getirildi."
        };
    }
}