using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using HappyDay.Persistance.Security;
using MediatR;

// Eğer hash kullanıyorsan:


namespace HappyDay.Application.Features.Queries.AutLogin
{
    public class AuthLoginQueryRequestHandler
        : IRequestHandler<AuthLoginQueryRequest, GeneralResponse<AuthLoginQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public AuthLoginQueryRequestHandler(
            IMapper mapper,
            IUserRepository userRepository,
            JwtService jwtService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<GeneralResponse<AuthLoginQueryResponse>> Handle(
            AuthLoginQueryRequest request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return new GeneralResponse<AuthLoginQueryResponse>
                {
                    Message = Messages.MessageConstants.InvalidUserData,
                    isSuccess = false
                };
            }

            // Parola doğrulama
            // Hash'li saklıyorsanız:
            // var passOk = BCryptNet.Verify(request.Password, user.PasswordHash);
            // Düz metin ise (önerilmez):
            var passOk = user.Password == request.Password;

            if (!passOk)
            {
                return new GeneralResponse<AuthLoginQueryResponse>
                {
                    Message = Messages.MessageConstants.InvalidUserData,
                    isSuccess = false
                };
            }

            // Rolünüz farklı bir yerde tutuluyorsa (user.Role gibi) onu da ekleyebilirsiniz.
            // Burada standart "User" rolü ile token üretiyoruz.
            var token = _jwtService.GenerateUserToken(user.Id.ToString());

            return new GeneralResponse<AuthLoginQueryResponse>
            {
                Message = Messages.MessageConstants.UserLogin,
                Data = new AuthLoginQueryResponse
                {
                    Token = token
                },
                isSuccess = true
            };
        }
    }
}
