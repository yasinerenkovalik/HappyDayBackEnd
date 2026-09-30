using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Admin.Dashboard;

public class GetAdminDashboardRequest : IRequest<GeneralResponse<GetAdminDashboardResponse>>
{
}

public class GetAdminDashboardResponse
{
    public int TotalCompanies { get; set; }
    public int PendingCompanies { get; set; }
    public int ApprovedCompanies { get; set; }
    public int TotalOrganizations { get; set; }
    public int TotalUsers { get; set; }
    public int TotalMessages { get; set; }
    public int MessagesLast7Days { get; set; }
    public int MessagesToday { get; set; }

    /// <summary>Son 7 günün günlük mesaj dağılımı (grafik için).</summary>
    public List<DailyMessageCount> DailyMessages { get; set; } = new();

    /// <summary>En çok mesaj alan ilk 5 şirket.</summary>
    public List<TopCompanyMessageCount> TopCompanies { get; set; } = new();
}

public class DailyMessageCount
{
    public string Date { get; set; } = default!;
    public int Count { get; set; }
}

public class TopCompanyMessageCount
{
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; } = default!;
    public int Count { get; set; }
}
