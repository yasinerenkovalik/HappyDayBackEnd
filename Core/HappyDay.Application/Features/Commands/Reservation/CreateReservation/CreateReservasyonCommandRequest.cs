using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Reservation.CreateReservation;

public class CreateReservasyonCommandRequest:IRequest<GeneralResponse<CreateReservasyonCommandResponse>>
{
    public DateTime ReservationDate { get; set; } // Tarih ve saat
    public int GuestCount { get; set; }
    public string Notes { get; set; }
    public Guid UserId { get; set; } // Customer
    public Guid CompanyId { get; set; }        // ✔ Foreign key
    public Guid OrganizationId { get; set; }
}