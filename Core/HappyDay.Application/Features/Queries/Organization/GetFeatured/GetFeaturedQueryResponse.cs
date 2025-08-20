namespace HappyDay.Application.Features.Queries.Organization.GetFeatured;

public class GetFeaturedQueryResponse
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Location { get; set; }
    public string Duration { get; set; }
    public bool IsOutdoor { get; set; }
    public string? CoverPhotoPath { get; set; }
    public Guid Id { get; set; }
}