using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BadmintonBookingSystem.Service.Services
{
    public class TokenService : ITokenService
    {
        private const int FIVE_MINUTES = 300;
        private readonly IConfiguration _configuration;
        private readonly UserManager<UserEntity> _userManager;
        public TokenService(IConfiguration configuration, UserManager<UserEntity> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public string CreateTokenForAccount(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Name,user.FullName)
            };

            var roles = _userManager.GetRolesAsync(user);
            foreach (var role in roles.Result)
            {
                claims.Add(new Claim("role", role));
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SECRET_KEY") ?? _configuration["jwt:secret"]));

            var credential = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _configuration.GetValue<string>("SECRET_ISSUER") ?? _configuration["jwt:issuer"],
                _configuration.GetValue<string>("SECRET_AUDIENCE") ?? _configuration["jwt:audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credential);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
