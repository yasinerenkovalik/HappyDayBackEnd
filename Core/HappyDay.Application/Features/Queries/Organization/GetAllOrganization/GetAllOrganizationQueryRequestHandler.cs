using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Application.Features.Queries.Organization.GetAllOrganization;

public class GetAllOrganizationQueryRequestHandler 
    : IRequestHandler<GetAllOrganizationQueryRequest, GeneralResponse<PagedResult<GetAllOrganizationQueryResponse>>>
{
    private readonly IOrganizationRepository _organizationRepository;

    public GetAllOrganizationQueryRequestHandler(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task<GeneralResponse<PagedResult<GetAllOrganizationQueryResponse>>> Handle(
        GetAllOrganizationQueryRequest request, 
        CancellationToken cancellationToken)
    {
        // GenericRepository<T>.GetPagedAsync<TResult>(...) projeksiyonlu overload
        var result = await _organizationRepository.GetPagedAsync<GetAllOrganizationQueryResponse>(
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            ct: cancellationToken,
            selector: q => q
                .Include(o => o.City)
                .Include(o => o.District)
                .Select(o => new GetAllOrganizationQueryResponse
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Price = o.Price,
                    MaxGuestCount = o.MaxGuestCount,
                   
                    CityName = o.City.CityName,
                 
                    DistrictName = o.District.DistrictName,
                    IsOutdoor = o.IsOutdoor,
                    Duration = o.Duration,
                    ReservationNote = o.ReservationNote,
                    CancelPolicy = o.CancelPolicy,
                    VideoUrl = o.VideoUrl,
                    CoverPhotoPath = o.CoverPhotoPath
                }),
            orderBy: q => q.OrderBy(o => o.Title)
        );


        if (result.Items == null || !result.Items.Any())
        {
            return new GeneralResponse<PagedResult<GetAllOrganizationQueryResponse>>
            {
                Message = Messages.MessageConstants.OrganizationNotFound,
                isSuccess = false
            };
        }

        // result zaten DTO ile döndü (tek sorgu). Ek map gerekmez.
        return new GeneralResponse<PagedResult<GetAllOrganizationQueryResponse>>
        {
            Message = Messages.MessageConstants.OrganizationGet, // ✅ başarı mesajı
            isSuccess = true,
            Data = result
        };
    }
}
