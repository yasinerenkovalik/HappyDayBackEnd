using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HappyDay.Application.Features.Commands.User.CreateUser;
using HappyDay.Application.Features.Queries.AutLogin;
using HappyDay.Application.Features.Queries.User.GetAllUser;
using HappyDay.Application.Features.Queries.User.GetByIdUser;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       
        private readonly IMediator _mediator;
        private IConfiguration _config;
        public UserController( IMediator mediator,IConfiguration configuration)
        {
           
            _mediator = mediator;
            _config = configuration;
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
        [HttpGet("validate-token")]
        public IActionResult ValidateToken(string token)
        {
            var jwtSettings = _config.GetSection("JwtSettings");

            var secretKeyString = jwtSettings["Secret"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            if (string.IsNullOrEmpty(secretKeyString) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                return BadRequest("JWT ayarları eksik!");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString));
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // İsteğe bağlı: Token süresi dolmuşsa anında expire eder
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                // Token başarılıysa, claim bilgilerini dön (örnek)
                var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var role = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

                return Ok(new
                {
                    IsValid = true,
                    UserId = userId,
                    Role = role
                });
            }
            catch (SecurityTokenExpiredException)
            {
                return Unauthorized("Token süresi dolmuş.");
            }
            catch (Exception)
            {
                return Unauthorized("Token geçersiz.");
            }
        }
    }
}
