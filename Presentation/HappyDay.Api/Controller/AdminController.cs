using HappyDay.Application.Features.Commands.Admin.SetCompanyApproval;
using HappyDay.Application.Features.Commands.Admin.SetUserActive;
using HappyDay.Application.Features.Queries.Admin.Dashboard;
using HappyDay.Application.Features.Queries.Admin.ManageCompanies;
using HappyDay.Application.Features.Queries.Admin.ManageContactMessages;
using HappyDay.Application.Features.Queries.Admin.ManageUsers;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller;

/// <summary>
/// Platform yonetim paneli. Tum endpoint'ler yalnizca Admin rolu ile erisilebilir.
/// </summary>
[Authorize(Roles = "Admin")]
[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("dashboard")]
    public async Task<GeneralResponse<GetAdminDashboardResponse>> GetDashboard()
    {
        return await _mediator.Send(new GetAdminDashboardRequest());
    }

    [HttpGet("companies")]
    public async Task<GeneralResponse<List<GetAdminCompaniesItem>>> GetCompanies(
        [FromQuery] GetAdminCompaniesRequest request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("companies/approval")]
    public async Task<GeneralResponse<SetCompanyApprovalResponse>> SetCompanyApproval(
        [FromForm] SetCompanyApprovalCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpGet("users")]
    public async Task<GeneralResponse<List<GetAdminUsersItem>>> GetUsers(
        [FromQuery] GetAdminUsersRequest request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("users/active")]
    public async Task<GeneralResponse<SetUserActiveResponse>> SetUserActive(
        [FromForm] SetUserActiveCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpGet("messages")]
    public async Task<GeneralResponse<List<GetAdminContactMessagesItem>>> GetMessages(
        [FromQuery] GetAdminContactMessagesRequest request)
    {
        return await _mediator.Send(request);
    }
}
