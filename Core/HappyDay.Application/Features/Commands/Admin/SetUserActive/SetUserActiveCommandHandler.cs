using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Admin.SetUserActive;

public class SetUserActiveCommandHandler
    : IRequestHandler<SetUserActiveCommand, GeneralResponse<SetUserActiveResponse>>
{
    private readonly IUserRepository _users;

    public SetUserActiveCommandHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<GeneralResponse<SetUserActiveResponse>> Handle(
        SetUserActiveCommand request,
        CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            return new GeneralResponse<SetUserActiveResponse>
            {
                isSuccess = false,
                Message = "Kullanici Id'si zorunludur."
            };
        }

        // Pasif kullanicilar da tekrar aktiflestirilebilmeli.
        var user = await _users.GetByIdIncludingInactiveAsync(request.UserId);

        if (user is null)
        {
            return new GeneralResponse<SetUserActiveResponse>
            {
                isSuccess = false,
                Message = "Kullanici bulunamadi."
            };
        }

        user.IsActivated = request.IsActivated;

        if (request.IsActivated)
        {
            user.DeleteDate = default;
        }

        await _users.UpdateAsync(user);

        return new GeneralResponse<SetUserActiveResponse>
        {
            Data = new SetUserActiveResponse
            {
                UserId = user.Id,
                IsActivated = user.IsActivated
            },
            isSuccess = true,
            Message = request.IsActivated ? "Kullanici aktiflestirildi." : "Kullanici pasiflestirildi."
        };
    }
}
