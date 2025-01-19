using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PlatformSport.Configrations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlatformSport.Helpers
{
    public class JwtTokenHelper
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenHelper(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            Console.WriteLine($"JwtSettings.Secret: {_jwtSettings.Secret}");
            Console.WriteLine($"JwtSettings.Issuer: {_jwtSettings.Issuer}");
        }

        public string GenerateToken(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null.");

            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentNullException(nameof(user.Email), "User email cannot be null or empty.");

            if (string.IsNullOrEmpty(user.Id))
                throw new ArgumentNullException(nameof(user.Id), "User ID cannot be null or empty.");

            // Log the JwtSettings to ensure they are populated correctly
            Console.WriteLine($"JwtSettings.Secret: {_jwtSettings.Secret}");
            Console.WriteLine($"JwtSettings.Issuer: {_jwtSettings.Issuer}");

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
