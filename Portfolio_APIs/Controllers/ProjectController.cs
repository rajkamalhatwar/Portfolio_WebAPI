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
    public class ProjectController : ControllerBase
    {
        IProjectService _IProjectService;
        public ProjectController(IProjectService iProjectService)
        {
            _IProjectService = iProjectService;
        }

        [HttpPost]
        [Route("SaveProjectInfo")]
        [Authorize]
        public async Task<IActionResult> SaveProjectInfo([FromBody] VMProject vMProject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 

            try
            {
                int res = await _IProjectService.SubmitProjectInfoAsync(vMProject);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "Project details saved successfully." }),
                    2 => Ok(new { Res = res, Message = "Project details updated successfully." }),
                    3 => Ok(new { Res = res, Message = "Project record already exists." }),
                    4 => Ok(new { Res = res, Message = "Project record not found for update." }),
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

        [HttpGet("GetProjects")]
        public async Task<IActionResult> GetProjectById([FromQuery] int? projectId = null, [FromQuery] int? userId = null)
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

            var result = await _IProjectService.GetProjectByIdAsync(projectId, finalUserId);

            if (result == null)
                return NotFound(new { Message = "Projects record not found." });

            return Ok(result);
        }


        [HttpPost]
        [Route("DeleteProject")]
        [Authorize]
        public async Task<IActionResult> DeleteProject([FromQuery] int projectId)
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst("userId")?.Value);
                int res = await _IProjectService.DeleteProjectById(projectId, userId);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "Project Record Delete successfully." }),
                    0 => Ok(new { Res = res, Message = "Project Record Not Found." }),
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
