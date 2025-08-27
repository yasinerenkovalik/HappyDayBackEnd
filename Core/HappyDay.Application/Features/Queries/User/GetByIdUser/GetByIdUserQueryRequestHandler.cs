using AutoMapper;
using HappyDay.Application.Features.Queries.User.GetAllUser;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.User.GetByIdUser;

public class GetByIdUserQueryRequestHandler:IRequestHandler<GetByIdUserQueryRequest, GeneralResponse<GetByIdUserQueryResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetByIdUserQueryRequestHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<GeneralResponse<GetByIdUserQueryResponse>> Handle(GetByIdUserQueryRequest request, CancellationToken cancellationToken)
    {
        var result= await _userRepository.GetByIdAsync(request.Id);
        return new GeneralResponse<GetByIdUserQueryResponse>()
        {
            Data = _mapper.Map<GetByIdUserQueryResponse>(result),
            Message = Messages.MessageConstants.UserById,
            isSuccess = true
        };

    }
}