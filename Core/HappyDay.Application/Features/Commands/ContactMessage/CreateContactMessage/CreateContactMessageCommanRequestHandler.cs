using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.ContactMessage.CreateContactMessage
{
    public class CreateContactMessageCommanRequestHandler
        : IRequestHandler<CreateContactMessageCommanRequest, GeneralResponse<CreateContactMessageCommanResponse>>
    {
        private readonly IContactMessageRepository _contactMessageRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IMapper _mapper;

        public CreateContactMessageCommanRequestHandler(
            IContactMessageRepository contactMessageRepository,
            IMapper mapper,
            IOrganizationRepository organizationRepository)
        {
            _contactMessageRepository = contactMessageRepository ?? throw new ArgumentNullException(nameof(contactMessageRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
        }

        public async Task<GeneralResponse<CreateContactMessageCommanResponse>> Handle(
            CreateContactMessageCommanRequest request, CancellationToken cancellationToken)
        {
            if (request == null || request.OrganizationId == Guid.Empty)
            {
                return new GeneralResponse<CreateContactMessageCommanResponse>
                {
                    isSuccess = false,
                    Message = "Geçersiz istek.",
                    Errors = new[] { "Request null veya OrganizationId boş." }
                };
            }

            // Sadece CompanyId çek (navigation'a gerek yok)
            var org = await _organizationRepository.GetByIdAsync(request.OrganizationId);
            if (org == null)
            {
                return new GeneralResponse<CreateContactMessageCommanResponse>
                {
                    isSuccess = false,
                    Message = "Organization bulunamadı.",
                    Errors = new[] { $"Organization not found: {request.OrganizationId}" }
                };
            }

            var companyId = org.CompanyId; // <-- kritik düzeltme
            if (companyId == Guid.Empty)
            {
                return new GeneralResponse<CreateContactMessageCommanResponse>
                {
                    isSuccess = false,
                    Message = "Organization'a bağlı şirket bulunamadı.",
                    Errors = new[] { "CompanyId boş." }
                };
            }

            var contactMessage = _mapper.Map<Domain.Entities.ContactMessage>(request);
            contactMessage.CompanyId = companyId;

            // Varsa FK'sını da set et (entity'nde OrganizationId alanı bulunuyorsa)
            var orgIdProp = contactMessage.GetType().GetProperty("OrganizationId");
            if (orgIdProp != null) orgIdProp.SetValue(contactMessage, request.OrganizationId);

            await _contactMessageRepository.AddAsync(contactMessage);

            return new GeneralResponse<CreateContactMessageCommanResponse>
            {
                isSuccess = true,
                Message = "New contact message created",
         
            };
        }
    }
}
