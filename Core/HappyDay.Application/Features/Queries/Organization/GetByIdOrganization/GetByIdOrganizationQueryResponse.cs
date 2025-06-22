namespace HappyDay.Application.Features.Queries.Organization.GetByIdOrganization;

public class GetByIdOrganizationQueryResponse
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int MaxGuestCount { get; set; }
    public string Location { get; set; }
    public List<string> Services { get; set; } = new();
    public string Duration { get; set; }
    public bool IsOutdoor { get; set; }
    public string ReservationNote { get; set; }
    public string CancelPolicy { get; set; }
    public string VideoUrl { get; set; }
    public Guid CompanyId { get; set; }
    public string? CoverPhotoPath { get; set; }
}