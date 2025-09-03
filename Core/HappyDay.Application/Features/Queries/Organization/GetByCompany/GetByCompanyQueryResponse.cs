namespace HappyDay.Application.Features.Queries.Organization.GetByCompany;

public class GetByCompanyQueryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int CategoryId { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public List<string> Services { get; set; } = new();
    public bool IsOutdoor { get; set; }
    public string? CoverPhotoPath { get; set; }

}