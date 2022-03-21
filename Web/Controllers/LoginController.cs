using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Dtos;
using Web.Services;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] LoginPostRequest loginPostRequest)
        {
            var token = await _loginService.GetTokenAsync(loginPostRequest.Login, loginPostRequest.Password);
            return Ok(token);
        }
    }
}