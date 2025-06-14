namespace HappyDay.Application.Features.Queries.Organization.GetByIdOrganization;

public class GetByIdOrganizationQueryResponse
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int MaxGuestCount { get; set; }
    public Guid CompanyId { get; set; }
}