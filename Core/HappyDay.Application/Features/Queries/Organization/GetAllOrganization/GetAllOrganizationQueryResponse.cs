namespace HappyDay.Application.Features.Queries.Organization.GetAllOrganization;

public class GetAllOrganizationQueryResponse
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int MaxGuestCount { get; set; }
    public string? CoverPhotoPath { get; set; }
    public Guid CompanyId { get; set; }
    public Guid Id { get; set; }
}