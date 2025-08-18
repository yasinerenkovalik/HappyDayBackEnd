using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Company.GetAllCompany;

public class GetAllCompanyQueryRequestHandler: IRequestHandler<GetAllCompanyQueryRequest, GeneralResponse<List<GetAllCompanyQueryResponse>>>
{
    private  readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public GetAllCompanyQueryRequestHandler(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<List<GetAllCompanyQueryResponse>>> Handle(GetAllCompanyQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _companyRepository.GetAllAysnc();
        return new GeneralResponse<List<GetAllCompanyQueryResponse>>()
        {
            Data = _mapper.Map<List<GetAllCompanyQueryResponse>>(result),
            Message = Messages.MessageConstants.UserById,
            isSuccess = true
        };
    }
}