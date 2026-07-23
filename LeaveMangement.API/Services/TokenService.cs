using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LeaveMangement.API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LeaveMangement.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GenerateToken(
   string userId,
    int employeeId,
    string email,
    string role)
        {
            var claims = new[]
{
    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),

    new Claim("EmployeeId", employeeId.ToString()),

    new Claim(JwtRegisteredClaimNames.Email, email),

    new Claim(ClaimTypes.Role, role)
};


            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                ));


            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],

                audience: _configuration["Jwt:Audience"],

                claims: claims,

                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(
                        _configuration["Jwt:ExpiryMinutes"]
                    )),

                signingCredentials: credentials
            );


            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}