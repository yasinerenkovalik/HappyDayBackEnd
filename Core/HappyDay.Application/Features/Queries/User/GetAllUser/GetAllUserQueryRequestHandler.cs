using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.User.GetAllUser;

public class GetAllUserQueryRequestHandler:IRequestHandler<GetAllUserQueryRequest, GeneralResponse<List<GetAllUserQueryResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllUserQueryRequestHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public  async Task<GeneralResponse<List<GetAllUserQueryResponse>>> Handle(GetAllUserQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.GetAllAysnc();
        return new GeneralResponse<List<GetAllUserQueryResponse>>()
        {
            Message = Messages.MessageConstants.UserGetAll,
            Data = _mapper.Map<List<GetAllUserQueryResponse>>(result),
            isSuccess = true
        };
    }
}