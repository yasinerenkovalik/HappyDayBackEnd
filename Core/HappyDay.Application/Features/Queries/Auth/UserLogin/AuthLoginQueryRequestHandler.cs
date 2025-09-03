using AutoMapper;
using HappyDay.Application.Common.Security;           // IPasswordHasher
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using HappyDay.Persistance.Security;                  // JwtService
using MediatR;

namespace HappyDay.Application.Features.Queries.AutLogin
{
    public class AuthLoginQueryRequestHandler
        : IRequestHandler<AuthLoginQueryRequest, GeneralResponse<AuthLoginQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthLoginQueryRequestHandler(
            IMapper mapper,
            IUserRepository userRepository,
            JwtService jwtService,
            IPasswordHasher passwordHasher)             // <-- bcrypt hasher'ı enjekte ediyoruz
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<GeneralResponse<AuthLoginQueryResponse>> Handle(
            AuthLoginQueryRequest request,
            CancellationToken cancellationToken)
        {
            // E-postayı normalize et
            var email = (request.Email ?? string.Empty).Trim().ToLowerInvariant();

            // Kullanıcıyı getir (repo metodun ct almıyorsa ikinci argümanı kaldır)
            var user = await _userRepository.GetByEmailAsync(email /*, cancellationToken*/);
            if (user is null || string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                return new GeneralResponse<AuthLoginQueryResponse>
                {
                    Message = Messages.MessageConstants.InvalidUserData,
                    isSuccess = false
                };
            }

            // BCRYPT doğrulaması
            var passOk = _passwordHasher.Verify(request.Password ?? string.Empty, user.PasswordHash);
            if (!passOk)
            {
                return new GeneralResponse<AuthLoginQueryResponse>
                {
                    Message = Messages.MessageConstants.InvalidUserData,
                    isSuccess = false
                };
            }

            // JWT üret (rol vs. eklemek istersen burada claim ekleyebilirsin)
            var token = _jwtService.GenerateUserToken(user.Id.ToString());

            return new GeneralResponse<AuthLoginQueryResponse>
            {
                Message = Messages.MessageConstants.UserLogin,
                Data = new AuthLoginQueryResponse { Token = token },
                isSuccess = true
            };
        }
    }
}
