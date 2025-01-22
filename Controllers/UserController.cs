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
        private readonly IWebHostEnvironment _env; // Add this field


        public UserController(IUserService userService, IWebHostEnvironment env)
        {
            _userService = userService;
            _env = env;
        }


        [HttpPost("UploadProfilePicture")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Validate file type (e.g., allow only images)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
            }

            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(_env.WebRootPath, "profile-pictures", fileName);

            // Ensure the directory exists
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Update the user's profile picture URL
            var profilePictureUrl = $"/profile-pictures/{fileName}";
            var result = await _userService.UpdateProfilePictureAsync(userId, profilePictureUrl);

            if (!result)
            {
                return StatusCode(500, "Failed to update profile picture.");
            }

            return Ok(new { ProfilePictureUrl = profilePictureUrl });
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

        // UserController.cs
        [HttpGet("Rooms/Created")]
        [Authorize]
        public async Task<IActionResult> GetRoomsCreatedByUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rooms = await _userService.GetRoomsCreatedByUserAsync(userId);
            return Ok(rooms);
        }

        [HttpGet("Rooms/Joined")]
        [Authorize]
        public async Task<IActionResult> GetRoomsJoinedByUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rooms = await _userService.GetRoomsJoinedByUserAsync(userId);
            return Ok(rooms);
        }
    }
}
