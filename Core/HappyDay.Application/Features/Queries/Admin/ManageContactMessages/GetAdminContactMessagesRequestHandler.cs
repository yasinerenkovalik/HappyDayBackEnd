using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Admin.ManageContactMessages;

public class GetAdminContactMessagesRequestHandler
    : IRequestHandler<GetAdminContactMessagesRequest, GeneralResponse<List<GetAdminContactMessagesItem>>>
{
    private readonly IContactMessageRepository _messages;

    public GetAdminContactMessagesRequestHandler(IContactMessageRepository messages)
    {
        _messages = messages;
    }

    public async Task<GeneralResponse<List<GetAdminContactMessagesItem>>> Handle(
        GetAdminContactMessagesRequest request,
        CancellationToken cancellationToken)
    {
        var page = request.PageNumber < 1 ? 1 : request.PageNumber;
        var size = request.PageSize is < 1 or > 200 ? 20 : request.PageSize;

        var items = await _messages.GetAllForAdmin(request.Search, (page - 1) * size, size);

        var response = items.Select(m => new GetAdminContactMessagesItem
        {
            Id = m.Id,
            FullName = m.FullName,
            Phone = m.Phone,
            Email = m.Email,
            Message = m.Message,
            CreateDate = m.CreateDate,
            CompanyId = m.CompanyId,
            CompanyName = m.CompanyName,
            OrganizationTitle = m.OrganizationTitle,
            CityName = m.CityName,
            DistrictName = m.DistrictName
        }).ToList();

        return new GeneralResponse<List<GetAdminContactMessagesItem>>
        {
            Data = response,
            isSuccess = true,
            Message = "Mesaj listesi getirildi."
        };
    }
}
