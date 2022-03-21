using System;
using System.Threading.Tasks;
using BusinessLayer.Services;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Dtos;
using AuthorizationPolicy = Web.Auth.AuthorizationPolicy;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId:int}")]
        [Authorize(AuthorizationPolicy.User)]
        public async Task<ActionResult<User>> Get(int userId)
        {
            if (userId <= 0)
            {
                return NotFound();
            }

            var user = await _userService.GetAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        [Authorize(AuthorizationPolicy.User)]
        public async Task<ActionResult<User>> Get()
        {
            var users = await _userService.GetAllAsync();

            // Можно было бы возвращать нормальные дтошки, но для этого надо бы знать какие данные понадобятся клинту.
            return Ok(users);
        }

        [HttpPost]
        [Authorize(AuthorizationPolicy.Admin)]
        public async Task<ActionResult<int>> Post([FromBody] UserPostRequest userPostRequest)
        {
            var newUserId = await _userService.AddUserAsync(userPostRequest.Login, userPostRequest.Password);
            return Ok(newUserId);
        }

        [HttpDelete("{userId:int}")]
        [Authorize(AuthorizationPolicy.Admin)]
        public async Task<ActionResult> Delete(int userId)
        {
            if (userId <= 0)
            {
                return NotFound();
            }

            try
            {
                await _userService.RemoveUserAsync(userId);
            }
            catch (ApplicationException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}