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
    public class CreativeWorksController : ControllerBase
    {
        private readonly ICreativeWorksService _ICreativeWorksService;

        public CreativeWorksController(ICreativeWorksService creativeWorksService)
        {
            _ICreativeWorksService = creativeWorksService;
        }

        [HttpPost]
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
        public async Task<IActionResult> GetWorkCategory([FromQuery] int? workCategoryId = null)
        {
            int userId = Convert.ToInt32(User.FindFirst("userId")?.Value);

            var result = await _ICreativeWorksService.GetWorkCategaryByIdAsync(workCategoryId, userId);

            if (result == null)
                return NotFound(new { Message = "Work Categary record not found." });

            return Ok(result);
        }

        [HttpPost]
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
    }
}
