using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test : ControllerBase
    {
        [HttpGet("GetOrganizationWithICompany")]
        public IActionResult GetOrganizationWithICompany()
        {
            MailService mailService = new MailService();
            mailService.SendAsync("erenkovalik42@gmail.com", "deneme ", "deneme");

            return Ok("deneme başarılı");
        }
    }
}
