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
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly MailService _mailService;

        public CreateContactMessageCommanRequestHandler(
            IContactMessageRepository contactMessageRepository,
            IMapper mapper,
            IOrganizationRepository organizationRepository,
            ICompanyRepository companyRepository,
            MailService mailService)
        {
            _contactMessageRepository = contactMessageRepository ?? throw new ArgumentNullException(nameof(contactMessageRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
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

            // Organization kontrolü
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

            // Organization'a bağlı şirket bilgisi
            var companyId = org.CompanyId;
            if (companyId == Guid.Empty)
            {
                return new GeneralResponse<CreateContactMessageCommanResponse>
                {
                    isSuccess = false,
                    Message = "Organization'a bağlı şirket bulunamadı.",
                    Errors = new[] { "CompanyId boş." }
                };
            }

            // ContactMessage kaydı
            var contactMessage = _mapper.Map<Domain.Entities.ContactMessage>(request);
            contactMessage.CompanyId = companyId;

            // OrganizationId varsa set et
            var orgIdProp = contactMessage.GetType().GetProperty("OrganizationId");
            if (orgIdProp != null)
                orgIdProp.SetValue(contactMessage, request.OrganizationId);

            await _contactMessageRepository.AddAsync(contactMessage);

            // Şirket bilgisi ve e-posta gönderimi
            var companyInfo = await _companyRepository.GetByIdAsync(companyId);
            if (companyInfo != null && !string.IsNullOrEmpty(companyInfo.Email))
            {
                await _mailService.SendAsync(
                    companyInfo.Email,
                    "Yeni Mesajınız Var",
                    $"<p>{companyInfo.Name} size bir mesaj gönderdi.</p><p><b>Mesaj:</b> {request.Message}</p>"
                );
            }

            return new GeneralResponse<CreateContactMessageCommanResponse>
            {
                isSuccess = true,
                Message = "New contact message created and email sent"
            };
        }
    }
}
