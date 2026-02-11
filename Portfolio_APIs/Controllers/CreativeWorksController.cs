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
    public class CreativeWorksController : ControllerBase
    {
        private readonly ICreativeWorksService _ICreativeWorksService;

        public CreativeWorksController(ICreativeWorksService creativeWorksService)
        {
            _ICreativeWorksService = creativeWorksService;
        }

        [HttpPost]
        [Authorize]
        [Route("SaveWorkCategoryInfo")]
        public async Task<IActionResult> SaveWorkCategoryInfo([FromBody] VMWorkCatogory vMWorkCatogory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int res = await _ICreativeWorksService.SubmitWorkCategaryInfoAsync(vMWorkCatogory);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "Work Category details saved successfully." }),
                    2 => Ok(new { Res = res, Message = "Work Category details updated successfully." }),
                    3 => Ok(new { Res = res, Message = "Work Category record Not Found for update." }),
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
        [Authorize]
        [Route("DeleteWorkCategory")]
        public async Task<IActionResult> DeleteWorkCategory([FromQuery] int workCategoryId)
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst("userId")?.Value);
                int res = await _ICreativeWorksService.DeleteWorkCategaryById(workCategoryId, userId);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "Work Category Record Delete successfully." }),
                    0 => Ok(new { Res = res, Message = "Work Category Record Not Found." }),
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

        [HttpGet("GetWorkCategory")]
        public async Task<IActionResult> GetWorkCategory([FromQuery] int? workCategoryId = null, [FromQuery] int? userId = null)
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

            var result = await _ICreativeWorksService.GetWorkCategaryByIdAsync(workCategoryId, finalUserId);

            if (result == null)
                return NotFound(new { Message = "Work Categary record not found." });

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("SaveCreativeWorkInfo")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SaveCreativeWorkInfo([FromForm] VMCreativeWork vMCreativeWork)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int res = await _ICreativeWorksService.SubmitCreativeWorksInfoAsync(vMCreativeWork);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "Work details saved successfully." }),
                    2 => Ok(new { Res = res, Message = "Work details updated successfully." }),
                    3 => Ok(new { Res = res, Message = "Work record Not Found for update." }),
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

        [HttpGet("GetCreativeWork")]
        public async Task<IActionResult> GetCreativeWork([FromQuery] int? workCategoryId = null, [FromQuery] int? userId = null)
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

            var result = await _ICreativeWorksService.GetCreativeWork(workCategoryId, finalUserId);

            if (result == null)
                return NotFound(new { Message = "Creative Work record not found." });

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("DeleteCreativeWork")]
        public async Task<IActionResult> DeleteCreativeWork([FromQuery] int creativeWorkId)
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst("userId")?.Value);
                int res = await _ICreativeWorksService.DeleteCreativeWorkById(creativeWorkId, userId);

                return res switch
                {
                    1 => Ok(new { Res = res, Message = "Creative Work Record Delete successfully." }),
                    0 => Ok(new { Res = res, Message = "Creative Work Record Not Found." }),
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
