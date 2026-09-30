using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using HappyDay.Domain.Entities;
using MediatR;

namespace HappyDay.Application.Features.Commands.Admin.SetCompanyApproval;

public class SetCompanyApprovalCommandHandler
    : IRequestHandler<SetCompanyApprovalCommand, GeneralResponse<SetCompanyApprovalResponse>>
{
    private readonly ICompanyRepository _companies;

    public SetCompanyApprovalCommandHandler(ICompanyRepository companies)
    {
        _companies = companies;
    }

    public async Task<GeneralResponse<SetCompanyApprovalResponse>> Handle(
        SetCompanyApprovalCommand request,
        CancellationToken cancellationToken)
    {
        if (request.CompanyId == Guid.Empty)
        {
            return new GeneralResponse<SetCompanyApprovalResponse>
            {
                isSuccess = false,
                Message = "Firma Id'si zorunludur."
            };
        }

        var company = await _companies.GetByIdAsync(request.CompanyId);

        if (company is null || !company.IsActivated)
        {
            return new GeneralResponse<SetCompanyApprovalResponse>
            {
                isSuccess = false,
                Message = "Firma bulunamadi."
            };
        }

        company.IsApproved = request.IsApproved;
        company.UpdateDate = DateTime.UtcNow;

        await _companies.UpdateAsync(company);

        return new GeneralResponse<SetCompanyApprovalResponse>
        {
            Data = new SetCompanyApprovalResponse
            {
                CompanyId = company.Id,
                IsApproved = company.IsApproved
            },
            isSuccess = true,
            Message = request.IsApproved ? "Firma onaylandi." : "Firma onayi kaldirildi."
        };
    }
}
