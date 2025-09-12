namespace HappyDay.Application.Features.Queries.Auth.OrganizationLogin;

public class CompanyLoginQueryResponse
{
    public string Token { get; set; }
    public bool IsEmailConfirmed { get; set; }
}