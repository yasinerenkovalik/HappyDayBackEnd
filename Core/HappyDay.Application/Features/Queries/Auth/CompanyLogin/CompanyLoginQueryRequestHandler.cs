using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using HappyDay.Persistance.Security;
using MediatR;

namespace HappyDay.Application.Features.Queries.Auth.OrganizationLogin;

public class CompanyLoginQueryRequestHandler:IRequestHandler<CompanyLoginQueryRequest, GeneralResponse<CompanyLoginQueryResponse>>
{
    private readonly ICompanyRepository  _companyRepository;
    private readonly JwtService  _jwtService;

    public CompanyLoginQueryRequestHandler(ICompanyRepository companyRepository, JwtService jwtService)
    {
        _companyRepository = companyRepository;
        _jwtService = jwtService;
    }

    public async Task<GeneralResponse<CompanyLoginQueryResponse>> Handle(CompanyLoginQueryRequest request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByEmailAsync(request.Email);
        if (company == null)  return new GeneralResponse<CompanyLoginQueryResponse>(){Message = Messages.MessageConstants.InvalidUserData};
        ;
        if (company.Password != request.Password)
            return new GeneralResponse<CompanyLoginQueryResponse>(){Message = Messages.MessageConstants.InvalidCompanyData};
        var token =_jwtService.GenerateToken(company.Id.ToString(),"company");
       

        return new GeneralResponse<CompanyLoginQueryResponse>()
        {
            Message = Messages.MessageConstants.CompanyLogin,
            Data = new CompanyLoginQueryResponse
            {
                Token = token,
                
            },
            isSuccess = true
            
        };
    }
}