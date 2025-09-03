using AutoMapper;
using HappyDay.Application.Features.Queries.Category.GetAllCategory;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("OrganizationGetAll")]
        public async Task<GeneralResponse<List<GetAllCategoryQueryResponse>>> OrganizationGetAll()
        {
            return await _mediator.Send(new GetAllCategoryQueryRequest());
        }
    }
}
