using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.ServiceInterfaces;
using ProjectAPI.ViewModel;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] VMAuth model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { Success = false, Message = "Invalid login request." });
            }

            var response = _authService.Login(model);  

            if (response.Success)
            {
                var token = response.Token;

                // Set token as HttpOnly Cookie
                Response.Cookies.Append("jwt", token, new CookieOptions
                { 
                    HttpOnly = false,   // JS can't read it
                    Secure = false,     // only HTTPS    on production it will be true
                    SameSite = SameSiteMode.Strict, // prevents CSRF for production 
                    Expires = DateTime.UtcNow.AddHours(1) // match token expiry
                });
                return Ok(response);
            }
            else
            {
                return Unauthorized(new { Success = false, Message = response.Message });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Remove cookie by overwriting with expired one
            Response.Cookies.Append("jwt", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1), // expire immediately
                HttpOnly = false,
                Secure = false, // true in production
                SameSite = SameSiteMode.Strict
            });

            return Ok(new { Success = true, Message = "Logged out successfully." });
        }

    }
}
    