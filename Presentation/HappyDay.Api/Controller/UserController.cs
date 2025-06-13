using HappyDay.Application.Features.Commands.User.CreateUser;
using HappyDay.Application.Features.Queries.AutLogin;
using HappyDay.Application.Features.Queries.User.GetAllUser;
using HappyDay.Application.Features.Queries.User.GetByIdUser;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       
        private readonly IMediator _mediator;
        public UserController( IMediator mediator)
        {
           
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<GeneralResponse<AuthLoginQueryResponse>>  Login(AuthLoginQueryRequest request)
        {
           return await _mediator.Send(request);
            
        }
        [HttpPost("create")]
        public async Task<GeneralResponse<CreateUserCommandResponse>>  Create(CreateUserCommandRequest request)
        {
            return await _mediator.Send(request);
            
        }
        [HttpGet("getall")]
        public async Task<GeneralResponse<List<GetAllUserQueryResponse>>>  GetAll( )
        {
            return await _mediator.Send(new  GetAllUserQueryRequest());
            
        }
        [HttpPost("getbyid")]
        public async Task<GeneralResponse<GetByIdUserQueryResponse>>  Getbyid(GetByIdUserQueryRequest request)
        {
            return await _mediator.Send(request);
            
        }
    }
}
