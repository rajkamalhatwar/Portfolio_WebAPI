using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Interfaces;
using ProjectAPI.ServiceInterfaces;
using ProjectAPI.Services;
using ProjectAPI.ViewModel;
using System.Threading.Tasks;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegController : ControllerBase
    {
        IUserRegService _IUserRegService;
        public UserRegController(IUserRegService iUserRegService)
        {
            _IUserRegService = iUserRegService;
        }

        [HttpPost]
        [Route("SaveUser")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SaveUser([FromForm] VMUserReg vMUserReg)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                long res = await _IUserRegService.SaveUser(vMUserReg);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "User registered successfully." }),
                    2 => Ok(new { Res = res, Message = "User updated successfully." }),
                    3 => Ok(new { Res = res, Message = "User with this email already exists." }),
                    4 => Ok(new { Res = res, Message = "User not found for update." }),
                    -99 => StatusCode(500, new { Res = res, Message = "An unexpected error occurred." }),
                    _ => StatusCode(500, new { Res = res, Message = "Unknown response from server." })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server error", Error = ex.Message });
            }
        }

        [HttpGet("GetAllUsers")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _IUserRegService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById([FromQuery] int? userId = null)
        {
            int finalUserId;

            // Case 1: Authorized request → get userId from JWT
            if (User.Identity?.IsAuthenticated == true)
            {
                finalUserId = Convert.ToInt32(User.FindFirst("userId")?.Value);
            }
            // Case 2: Unauthorized request → get userId from query param
            else if (userId.HasValue)
            {
                finalUserId = userId.Value;
            }
            // Case 3: Neither JWT nor userId provided
            else
            {
                return BadRequest(new { Message = "userId is required." });
            }
            VMUserRegOperations vMUserReg = new VMUserRegOperations();
            try
            {
                vMUserReg = await _IUserRegService.GetUsersById(finalUserId);
            }
            catch (Exception)
            {

            }
            return Ok(vMUserReg); 
     
        }


    }
}
