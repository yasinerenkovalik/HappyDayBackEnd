using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Company.GetByIdCompany;

public class GetByIdCompanyQueryRequestHandler:IRequestHandler<GetByIdCompanyQueryRequest,GeneralResponse< GetByIdCompanyQueryResponse>>
{
    private readonly IMapper _mapper;
    private readonly ICompanyRepository _companyRepository;

    public GetByIdCompanyQueryRequestHandler(IMapper mapper, ICompanyRepository companyRepository)
    {
        _mapper = mapper;
        _companyRepository = companyRepository;
    }

    public async Task<GeneralResponse<GetByIdCompanyQueryResponse>> Handle(GetByIdCompanyQueryRequest request, CancellationToken cancellationToken)
    {
        var result= await _companyRepository.GetByIdAsync(request.Id);
        return new GeneralResponse<GetByIdCompanyQueryResponse>()
        {
            Data = _mapper.Map<GetByIdCompanyQueryResponse>(result),
            Message = Messages.MessageConstants.UserById,
            isSuccess = true
        };
    }
}