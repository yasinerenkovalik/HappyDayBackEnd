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

            return Ok("deneme başarılı");
        }
    }
}
