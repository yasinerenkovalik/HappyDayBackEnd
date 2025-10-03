using System;
using System.Threading;
using System.Threading.Tasks;
using HappyDay.Application.Features.Commands.Company.CreateCompany; // Şu an generic bu tipi istiyor
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.CalendarEvent.CreateCalendarEvent
{
    public class CreateCalenderEventCommandRequestHandler
        : IRequestHandler<CreateCalenderEventCommandRequest, GeneralResponse<CreateCalenderEventCommandResponse>>
    {
        private readonly ICalanderEvenetRepository _calanderEvenetRepository;

        public CreateCalenderEventCommandRequestHandler(ICalanderEvenetRepository calanderEvenetRepository)
        {
            _calanderEvenetRepository = calanderEvenetRepository;
        }

        public async Task<GeneralResponse<CreateCalenderEventCommandResponse>> Handle(
            CreateCalenderEventCommandRequest request,
            CancellationToken cancellationToken)
        {
            // Basit doğrulama
            if (request.EndUtc <= request.StartUtc)
            {
                return new GeneralResponse<CreateCalenderEventCommandResponse>
                {
                    isSuccess = false,
                    Message = "Bitiş tarihi başlangıçtan sonra olmalıdır."
                };
            }

           

            // Entity eşle
            var entity = new Domain.Models.CalendarEvent
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                StartUtc = request.StartUtc,
                EndUtc = request.EndUtc,
                IsAllDay = request.IsAllDay,
                IsPublic = request.IsPublic,
                CompanyId = request.CompanyId, // yoksa null bırak
                CreateDate = DateTime.UtcNow
            };

            // Kaydet
            await _calanderEvenetRepository.AddAsync(entity);
            

           
            return new GeneralResponse<CreateCalenderEventCommandResponse>
            {
                isSuccess = true,
                Message = $"Takvim etkinliği oluşturuldu. Id: {entity.Id}"
            };
        }
    }
}
