using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.ContactMessage.GetByCompanyContactMessage;

public class GetByCompanyContactMessageRequestHandler 
    : IRequestHandler<GetByCompanyContactMessageRequest, GeneralResponse<List<GetByCompanyContactMessageResponse>>>
{
    private readonly IContactMessageRepository _contactMessageRepository;
    private readonly IMapper _mapper;

    public GetByCompanyContactMessageRequestHandler(IContactMessageRepository contactMessageRepository, IMapper mapper)
    {
        _contactMessageRepository = contactMessageRepository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<List<GetByCompanyContactMessageResponse>>> Handle(GetByCompanyContactMessageRequest request, CancellationToken cancellationToken)
    {
     
        var contactMessages = await _contactMessageRepository.GetByCompany(request.CompanyId);

        if (contactMessages == null || !contactMessages.Any())
        {
            return new GeneralResponse<List<GetByCompanyContactMessageResponse>>
            {
                Data = null,
                isSuccess = false,
                Message = "Bu şirkete ait mesaj bulunamadı."
            };
        }

      
        var response = _mapper.Map<List<GetByCompanyContactMessageResponse>>(contactMessages);

        return new GeneralResponse<List<GetByCompanyContactMessageResponse>>
        {
            Data = response,
            isSuccess = true,
            Message = "Şirkete ait mesajlar başarıyla getirildi."
        };
    }
}