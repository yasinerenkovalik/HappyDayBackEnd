using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Admin.ManageUsers;

public class GetAdminUsersRequestHandler
    : IRequestHandler<GetAdminUsersRequest, GeneralResponse<List<GetAdminUsersItem>>>
{
    private readonly IUserRepository _users;

    public GetAdminUsersRequestHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<GeneralResponse<List<GetAdminUsersItem>>> Handle(
        GetAdminUsersRequest request,
        CancellationToken cancellationToken)
    {
        var page = request.PageNumber < 1 ? 1 : request.PageNumber;
        var size = request.PageSize is < 1 or > 200 ? 20 : request.PageSize;

        var items = await _users.GetAllForAdmin(request.Search, (page - 1) * size, size);

        var response = items.Select(u => new GetAdminUsersItem
        {
            Id = u.Id,
            Name = u.Name,
            SurName = u.SurName,
            FullName = $"{u.Name} {u.SurName}".Trim(),
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            IsActivated = u.IsActivated,
            CreateDate = u.CreateDate,
            CompanyId = u.CompanyId,
            CompanyName = u.CompanyName
        }).ToList();

        return new GeneralResponse<List<GetAdminUsersItem>>
        {
            Data = response,
            isSuccess = true,
            Message = "Kullanici listesi getirildi."
        };
    }
}
