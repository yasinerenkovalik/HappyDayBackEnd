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
            
            bool passOk = company.Password == request.Password;

            if (!passOk)
            {
                return new GeneralResponse<CompanyLoginQueryResponse>
                {
                    Message = Messages.MessageConstants.InvalidCompanyData,
                    isSuccess = false
                };
            }

           
            var companyId = company.Id.ToString();
            var userId = company.Id.ToString(); 
        
            var token = _jwtService.GenerateCompanyToken(userId, companyId);
            
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
