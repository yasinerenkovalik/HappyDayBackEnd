namespace HappyDay.Application.Features.Queries.User.GetAllUser;

public class GetAllUserQueryResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string SurName { get; set; }
    public DateTime BirtDay { get; set; }
    public int IdentityNo { get; set; }
    public string Adress { get; set; }
    public string PhoneNumber { get; set; }
    public bool Sex { get; set; }
}