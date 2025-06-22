using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Category.GetAllCategory;

public class GetAllCategoryQueryRequestHandler:IRequestHandler<GetAllCategoryQueryRequest, GeneralResponse<List<GetAllCategoryQueryResponse>>>
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public GetAllCategoryQueryRequestHandler(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<List<GetAllCategoryQueryResponse>>> Handle(GetAllCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        var response = await _repository.GetAllAysnc();
        if (response == null)
        {
            return new GeneralResponse<List<GetAllCategoryQueryResponse>>()
            {
                Message = "Not found",
            };
        }
        
        return new GeneralResponse<List<GetAllCategoryQueryResponse>>()
        {
            Data = _mapper.Map<List<GetAllCategoryQueryResponse>>(response),
            Message = "Ok",
        };
    }
}