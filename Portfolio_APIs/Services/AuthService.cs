using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectAPI.Entity;
using ProjectAPI.Interfaces;
using ProjectAPI.ServiceInterfaces;
using ProjectAPI.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuth _authRepository; 
        private readonly IConfiguration _configuration;

        public AuthService(IAuth authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public LoginResponseViewModel Login(VMAuth model)
        {
            var user = _authRepository.GetUserByUsername(model.Username);
            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
            {
                return new LoginResponseViewModel
                {
                    Success = false,
                    Message = "Invalid username or password."
                };
            }

            // Generate JWT token here (not implemented in this snippet)
            string token = GenerateJwtToken(user);

            return new LoginResponseViewModel
            {
                Success = true,
                Message = "Login successful.",
                Token = token,
                UserId = user.UserId
            };
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            if (password == passwordHash)
                return true; // Placeholder
            else
                return false;
        }

        private string GenerateJwtToken(AuthEntity user)
        { 
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("Username", user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = tokenHandler.WriteToken(token);
            return jwtToken; // Placeholder
        } 
 
    }   

}
