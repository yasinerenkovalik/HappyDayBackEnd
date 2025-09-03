using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Category.GetAllCategory;

public class GetAllCategoryQueryRequest:IRequest<GeneralResponse<List<GetAllCategoryQueryResponse>>>
{
    
}