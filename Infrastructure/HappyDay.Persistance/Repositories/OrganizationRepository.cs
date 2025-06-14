using HappyDay.Application.Features.Queries.Organization.GetOrganizationWithImages;
using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class OrganizationRepository:GenericRepository<Organization>,IOrganizationRepository
{
    private readonly HappyDayContext _context;
    public OrganizationRepository(HappyDayContext appContext) : base(appContext)
    {
        _context = appContext;
    }

    public  async Task<GetOrganizationWithImagesResponse> GetOrganizationWithImages(Guid Id)
    {
        var result = await _context.Organizations
            .Where(o => o.Id == Id)
            .Select(o => new GetOrganizationWithImagesResponse
            {
                Id = o.Id,
                Title = o.Title,
                Description = o.Description,
                Price = o.Price,
                MaxGuestCount = o.MaxGuestCount,
                ImageUrls = o.OrganizationImages.Select(img => img.ImageUrl).ToList()
            })
            .FirstOrDefaultAsync();

        return result;
    }
}