using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Admin.ManageCompanies;

public class GetAdminCompaniesRequestHandler
    : IRequestHandler<GetAdminCompaniesRequest, GeneralResponse<List<GetAdminCompaniesItem>>>
{
    private readonly ICompanyRepository _companies;

    public GetAdminCompaniesRequestHandler(ICompanyRepository companies)
    {
        _companies = companies;
    }

    public async Task<GeneralResponse<List<GetAdminCompaniesItem>>> Handle(
        GetAdminCompaniesRequest request,
        CancellationToken cancellationToken)
    {
        var page = request.PageNumber < 1 ? 1 : request.PageNumber;
        var size = request.PageSize is < 1 or > 200 ? 20 : request.PageSize;

        var items = await _companies.GetAllForAdmin(
            request.Search, request.IsApproved, (page - 1) * size, size);

        var response = items.Select(c => new GetAdminCompaniesItem
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            PhoneNumber = c.PhoneNumber,
            Adress = c.Adress,
            IsApproved = c.IsApproved,
            IsEmailConfirmed = c.IsEmailConfirmed,
            IsActivated = c.IsActivated,
            CreateDate = c.CreateDate,
            OrganizationCount = c.OrganizationCount,
            UserCount = c.UserCount,
            MessageCount = c.MessageCount
        }).ToList();

        return new GeneralResponse<List<GetAdminCompaniesItem>>
        {
            Data = response,
            isSuccess = true,
            Message = "Firma listesi getirildi."
        };
    }
}
