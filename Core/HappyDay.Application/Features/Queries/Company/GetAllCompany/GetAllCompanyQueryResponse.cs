namespace HappyDay.Application.Features.Queries.Company.GetAllCompany;

public class GetAllCompanyQueryResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Adress { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }
    public Guid Id { get; set; }
}