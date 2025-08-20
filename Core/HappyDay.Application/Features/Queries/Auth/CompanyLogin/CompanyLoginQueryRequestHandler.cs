using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using HappyDay.Persistance.Security;
using MediatR;

// Eğer BCrypt kullanıyorsan:


namespace HappyDay.Application.Features.Queries.Auth.OrganizationLogin
{
    public class CompanyLoginQueryRequestHandler
        : IRequestHandler<CompanyLoginQueryRequest, GeneralResponse<CompanyLoginQueryResponse>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly JwtService _jwtService;

        public CompanyLoginQueryRequestHandler(
            ICompanyRepository companyRepository,
            JwtService jwtService)
        {
            _companyRepository = companyRepository;
            _jwtService = jwtService;
        }

        public async Task<GeneralResponse<CompanyLoginQueryResponse>> Handle(
            CompanyLoginQueryRequest request,
            CancellationToken cancellationToken)
        {
            // 1) Şirketi getir
            var company = await _companyRepository.GetByEmailAsync(request.Email);
            if (company is null)
            {
                return new GeneralResponse<CompanyLoginQueryResponse>
                {
                    Message = Messages.MessageConstants.InvalidUserData,
                    isSuccess = false
                };
            }

            // 2) Parola doğrulama
            // Eğer veritabanında HASHLI parola saklıyorsan:
            // bool passOk = BCryptNet.Verify(request.Password, company.PasswordHash);
            // Eğer düz metin (önerilmez) saklıyorsan:
            bool passOk = company.Password == request.Password;

            if (!passOk)
            {
                return new GeneralResponse<CompanyLoginQueryResponse>
                {
                    Message = Messages.MessageConstants.InvalidCompanyData,
                    isSuccess = false
                };
            }

            // 3) JWT üret
            // Not: GenerateCompanyToken(userId, companyId) bekliyor.
            // Elinde ayrıca bir UserId yoksa userId olarak company.Id kullanmak yeterli olur.
            var companyId = company.Id.ToString();
            var userId = company.Id.ToString(); // Eğer company.OwnerUserId varsa onu koy: company.OwnerUserId.ToString()

            // 👉 JwtService'teki ayrı fonksiyonu kullanıyoruz:
            var token = _jwtService.GenerateCompanyToken(userId, companyId);

            // 4) Response
            return new GeneralResponse<CompanyLoginQueryResponse>
            {
                Message = Messages.MessageConstants.CompanyLogin,
                Data = new CompanyLoginQueryResponse
                {
                    Token = token
                },
                isSuccess = true
            };
        }
    }
}
