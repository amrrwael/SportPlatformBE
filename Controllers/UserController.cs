using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlatformSport.Models.Dto;
using PlatformSport.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlatformSport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Deconstruct the tuple to get the token and error message
            var (token, errorMessage) = await _userService.RegisterAsync(dto);

            if (!string.IsNullOrEmpty(errorMessage))
                return BadRequest(new { message = errorMessage });

            return Ok(new { token });
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _userService.LoginAsync(dto.Email, dto.Password);

            if (token == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(new { token });
        }

        [HttpGet("Profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var profile = await _userService.GetUserProfileAsync(userId);

            if (profile == null)
                return NotFound(new { message = "User not found." });

            return Ok(profile);
        }

        [HttpPut("Profile")]
        [Authorize]
        public async Task<IActionResult> EditProfile([FromBody] UserProfileUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _userService.EditUserProfileAsync(userId, dto);

            if (!result)
                return NotFound(new { message = "User not found or update failed." });

            return Ok(new { message = "Profile updated successfully." });
        }
    }
}
