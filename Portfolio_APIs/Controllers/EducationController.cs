using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio_APIs.ServiceInterfaces;
using Portfolio_APIs.Services;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EducationController : ControllerBase
    {
        IEducationService _IEducationService;
        public EducationController(IEducationService iEducationService)
        {
            _IEducationService = iEducationService;
        }

        [HttpPost]
        [Route("SaveEducationInfo")]
        public async Task<IActionResult> SaveEducationInfo([FromBody] VMEducation vMEducation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int res = await _IEducationService.SubmitEducationInfoAsync(vMEducation);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "Education details saved successfully." }),
                    2 => Ok(new { Res = res, Message = "Education details updated successfully." }),
                    3 => Ok(new { Res = res, Message = "Education record already exists." }),
                    4 => Ok(new { Res = res, Message = "Education record not found for update." }),
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

        [HttpGet("GetEducation")]
        public async Task<IActionResult> GetEducationById([FromQuery] int? educationId = null)
        {
            int userId = Convert.ToInt32(User.FindFirst("userId")?.Value);

            var result = await _IEducationService.GetEducationByIdAsync(educationId, userId);

            if (result == null)
                return NotFound(new { Message = "Education record not found." });

            return Ok(result);
        }

        [HttpGet("GetSkills")]
        public async Task<IActionResult> GetSkillsByUserId()
        {
            // Get UserId from JWT claims
            int userId = Convert.ToInt32(User.FindFirst("userId")?.Value);

            var skills = await _IEducationService.GetSkillsByIdAsync(userId);

            if (skills == null || skills.Count == 0)
                return Ok(new List<VMSkill>());

            return Ok(skills);
        }

        [HttpPost]
        [Route("DeleteEducation")]
        public async Task<IActionResult> DeleteEducation([FromQuery] int educationId)
        { 
            try
            {
                int userId = Convert.ToInt32(User.FindFirst("userId")?.Value);
                int res = await _IEducationService.DeleteEducationById(educationId, userId);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "Education Record Delete successfully." }),
                    0 => Ok(new { Res = res, Message = "Education Record Not Found." }), 
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

        [HttpPost]
        [Route("SaveSkillInfo")]
        public async Task<IActionResult> SaveSkillInfo([FromBody] VMSkill vMSkill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int res = await _IEducationService.SubmitSkillInfo(vMSkill);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "Skills details saved successfully." }),
                    2 => Ok(new { Res = res, Message = "Skills details updated successfully." }),
                    3 => Ok(new { Res = res, Message = "Skills record Not Found for update." }), 
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

        [HttpPost]
        [Route("DeleteSkill")]
        public async Task<IActionResult> DeleteSkill([FromQuery] int skillId)
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst("userId")?.Value);
                int res = await _IEducationService.DeleteSkillById(skillId, userId);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "Skill Record Delete successfully." }),
                    0 => Ok(new { Res = res, Message = "Skill Record Not Found." }),
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
