using AutoMapper;
using FluentValidation;
using HappyDay.Application.Common.Security; // <-- IPasswordHasher
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.User.CreateUser;

public class CreateUserCommandRequestHandler
    : IRequestHandler<CreateUserCommandRequest, GeneralResponse<CreateUserCommandResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<CreateUserCommandRequest> _validator;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandRequestHandler(
        IMapper mapper,
        IUserRepository userRepository,
        IValidator<CreateUserCommandRequest> validator,
        IPasswordHasher passwordHasher) // <-- inject
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _validator = validator;
        _passwordHasher = passwordHasher;
    }

    public async Task<GeneralResponse<CreateUserCommandResponse>> Handle(
        CreateUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        // 1) Validasyon
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new GeneralResponse<CreateUserCommandResponse>
            {
                Message = Messages.MessageConstants.InvalidUserData,
                isSuccess = false
            };
        }

        // 2) Map + e-posta normalize
        var user = _mapper.Map<Domain.Entities.User>(request);
        if (!string.IsNullOrWhiteSpace(user.Email))
            user.Email = user.Email.Trim().ToLowerInvariant();
        var alreadyExists =
            (await _userRepository.GetByEmailAsync(user.Email)) is not null;
        

        if (alreadyExists)
        {
            return new GeneralResponse<CreateUserCommandResponse>
            {
                Message = "Bu e-posta ile zaten bir hesap var.",
                isSuccess = false
            };
        }

        // 3) Parolayı hash'le ve sadece hash'i sakla
        //    NOT: CreateUserCommandRequest içinde Password alanı olduğunu varsayıyoruz.
        //    Entity'de plain Password alanı bulunmamalı; sadece PasswordHash tutulmalı.
        user.PasswordHash = _passwordHasher.Hash(request.Password);

        // 4) Kaydet
        await _userRepository.AddAsync(user);

        return new GeneralResponse<CreateUserCommandResponse>
        {
            Message = Messages.MessageConstants.UserCreated,
            isSuccess = true
        };
    }
}
