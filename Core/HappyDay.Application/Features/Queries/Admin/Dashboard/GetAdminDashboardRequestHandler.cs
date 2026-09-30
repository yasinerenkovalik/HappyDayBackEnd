using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Admin.Dashboard;

public class GetAdminDashboardRequestHandler
    : IRequestHandler<GetAdminDashboardRequest, GeneralResponse<GetAdminDashboardResponse>>
{
    private readonly ICompanyRepository _companies;
    private readonly IUserRepository _users;
    private readonly IContactMessageRepository _messages;

    public GetAdminDashboardRequestHandler(
        ICompanyRepository companies,
        IUserRepository users,
        IContactMessageRepository messages)
    {
        _companies = companies;
        _users = users;
        _messages = messages;
    }

    public async Task<GeneralResponse<GetAdminDashboardResponse>> Handle(
        GetAdminDashboardRequest request,
        CancellationToken cancellationToken)
    {
        var (totalCompanies, approved, pending) = await _companies.GetApprovalCountsAsync();

        var allUsers = await _users.GetAllForAdmin(null, 0, 1);
        var allMessages = await _messages.GetAllForAdmin(null, 0, 500);
        var allCompanies = await _companies.GetAllForAdmin(null, null, 0, 500);

        var now = DateTime.UtcNow;
        var todayStart = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
        var last7 = now.AddDays(-7);

        var response = new GetAdminDashboardResponse
        {
            TotalCompanies = totalCompanies,
            ApprovedCompanies = approved,
            PendingCompanies = pending,
            TotalOrganizations = allCompanies.Sum(c => c.OrganizationCount),
            TotalUsers = allUsers.Count,
            TotalMessages = allMessages.Count,
            MessagesLast7Days = allMessages.Count(m => m.CreateDate >= last7),
            MessagesToday = allMessages.Count(m => m.CreateDate >= todayStart),
        };

        // Son 7 gunun gunluk dagilimi
        for (var i = 6; i >= 0; i--)
        {
            var day = todayStart.AddDays(-i);
            response.DailyMessages.Add(new DailyMessageCount
            {
                Date = day.ToString("dd MMM", System.Globalization.CultureInfo.InvariantCulture),
                Count = allMessages.Count(m => m.CreateDate.Date == day)
            });
        }

        // En cok mesaj alan sirketler
        response.TopCompanies = allMessages
            .GroupBy(m => new { m.CompanyId, m.CompanyName })
            .Select(g => new TopCompanyMessageCount
            {
                CompanyId = g.Key.CompanyId,
                CompanyName = g.Key.CompanyName,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        return new GeneralResponse<GetAdminDashboardResponse>
        {
            Data = response,
            isSuccess = true,
            Message = "Dashboard verileri getirildi."
        };
    }
}
