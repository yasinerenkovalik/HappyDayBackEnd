using AutoMapper;
using FluentValidation;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.User.CreateUser;

public class CreateUserCommandRequestHandler:IRequestHandler<CreateUserCommandRequest, GeneralResponse<CreateUserCommandResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<CreateUserCommandRequest> _validator;

    public CreateUserCommandRequestHandler(IMapper mapper, IUserRepository userRepository, IValidator<CreateUserCommandRequest> validator)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<GeneralResponse<CreateUserCommandResponse>> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new GeneralResponse<CreateUserCommandResponse>
            {
                Message = Messages.MessageConstants.InvalidUserData,
                isSuccess = false
            };
        }
        var organization = _mapper.Map<Domain.Entities.User>(request);
        await _userRepository.AddAsync(organization);
        return new  GeneralResponse<CreateUserCommandResponse>
        {
            Message = Messages.MessageConstants.UserCreated,
            isSuccess = true
        };
    }
}