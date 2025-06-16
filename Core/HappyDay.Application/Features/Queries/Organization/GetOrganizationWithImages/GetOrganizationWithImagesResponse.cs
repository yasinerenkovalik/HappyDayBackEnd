namespace HappyDay.Application.Features.Queries.Organization.GetOrganizationWithImages;

public class GetOrganizationWithImagesResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int MaxGuestCount { get; set; }
    public string? CoverPhotoPath { get; set; }
    public List<string> ImageUrls { get; set; }
}