using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Reservation.CreateReservation;

public class CreateReservasyonCommandRequestHandler:IRequestHandler<CreateReservasyonCommandRequest, GeneralResponse<CreateReservasyonCommandResponse>>
{
    public Task<GeneralResponse<CreateReservasyonCommandResponse>> Handle(CreateReservasyonCommandRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}