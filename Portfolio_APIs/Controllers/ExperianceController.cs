using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_APIs.Interfaces;
using Portfolio_APIs.ServiceInterfaces;
using Portfolio_APIs.Services;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExperianceController : ControllerBase
    {
        IExperianceService _IExperianceService;
        public ExperianceController(IExperianceService iExperianceService)
        {
            _IExperianceService = iExperianceService;
        }

        [HttpPost]
        [Route("SaveExperianceInfo")]
        public async Task<IActionResult> SaveExperianceInfo([FromBody] VMExperiance vMExperiance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int res = await _IExperianceService.SubmitExperianceInfoAsync(vMExperiance);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "Experiance details saved successfully." }),
                    2 => Ok(new { Res = res, Message = "Experiance details updated successfully." }),
                    3 => Ok(new { Res = res, Message = "Experiance record already exists." }),
                    4 => Ok(new { Res = res, Message = "Experiance record not found for update." }),
                    -99 => StatusCode(500, new { Res = res, Message = "An unexpected error occurred." }),
                    _ => StatusCode(500, new { Res = res, Message = "Unknown response from server." })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Server error",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("GetExperiance")]
        public async Task<IActionResult> GetEducationById([FromQuery] int? experianceId = null)
        {
            int userId = Convert.ToInt32(User.FindFirst("userId")?.Value);

            var result = await _IExperianceService.GetExperianceByIdAsync(experianceId, userId);

            if (result == null)
                return NotFound(new { Message = "Experiance record not found." });

            return Ok(result);
        }

        [HttpPost]
        [Route("DeleteExperience")]
        public async Task<IActionResult> DeleteEducation([FromQuery] int experianceId)
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst("userId")?.Value);
                int res = await _IExperianceService.DeleteExperianceById(experianceId, userId);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "Experience Record Delete successfully." }),
                    0 => Ok(new { Res = res, Message = "Experience Record Not Found." }),
                    -99 => StatusCode(500, new { Res = res, Message = "An unexpected error occurred." }),
                    _ => StatusCode(500, new { Res = res, Message = "Unknown response from server." })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Server error",
                    Error = ex.Message
                });
            }
        }
    }
}
