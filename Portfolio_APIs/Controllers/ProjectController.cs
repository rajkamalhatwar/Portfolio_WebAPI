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
    public class ProjectController : ControllerBase
    {
        IProjectService _IProjectService;
        public ProjectController(IProjectService iProjectService)
        {
            _IProjectService = iProjectService;
        }

        [HttpPost]
        [Route("SaveProjectInfo")]
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
        public async Task<IActionResult> GetProjectById([FromQuery] int? projectId = null)
        {
            int userId = Convert.ToInt32(User.FindFirst("userId")?.Value);

            var result = await _IProjectService.GetProjectByIdAsync(projectId, userId);

            if (result == null)
                return NotFound(new { Message = "Projects record not found." });

            return Ok(result);
        }


        [HttpPost]
        [Route("DeleteProject")]
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
