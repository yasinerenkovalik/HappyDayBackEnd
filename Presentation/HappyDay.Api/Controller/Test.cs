using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test : ControllerBase
    {
        private readonly MailService _mailService;
        private readonly ILogger<Test> _logger;

        public Test(MailService mailService, ILogger<Test> logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        // GUELIKLI: herkese mail gonderen acik endpoint. Kullanilmiyorsa butunu ilebilir.
        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrganizationWithICompany")]
        public async Task<IActionResult> GetOrganizationWithICompany()
        {
            await _mailService.SendAsync("erenkovalik42@gmail.com", "test 1232131231 ", "deneme");
            _logger.LogInformation("Test mail gönderimi tetiklendi.");

            return Ok("deneme başarılı");
        }
    }
}
