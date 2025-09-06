namespace HappyDay.Application.Features.Queries.Contact.GetAllContact;

public class GetAllContactQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Mesaage { get; set; }
}