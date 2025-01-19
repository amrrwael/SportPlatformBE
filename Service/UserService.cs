using Microsoft.AspNetCore.Identity;
using PlatformSport.Helpers;
using PlatformSport.Models.Dto;

namespace PlatformSport.Services
{
    public interface IUserService
    {
        Task<(string token, string errorMessage)> RegisterAsync(UserRegistrationDto dto);
        Task<string> LoginAsync(string email, string password);
        Task<UserProfileDto> GetUserProfileAsync(string userId);
        Task<bool> EditUserProfileAsync(string userId, UserProfileUpdateDto dto);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            JwtTokenHelper jwtTokenHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenHelper = jwtTokenHelper;
        }

        public async Task<(string token, string errorMessage)> RegisterAsync(UserRegistrationDto dto)
        {
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.Name,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return (null, errorMessage);
            }

            // Generate JWT token for the registered user
            var token = _jwtTokenHelper.GenerateToken(user);

            return (token, null);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return "Email and password are required.";
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return "Invalid email or password.";

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
                return "Invalid email or password.";

            return _jwtTokenHelper.GenerateToken(user);
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            return new UserProfileDto
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<bool> EditUserProfileAsync(string userId, UserProfileUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            user.FullName = dto.Name ?? user.FullName;
            user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
