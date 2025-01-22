using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlatformSport.Models.Dto;
using PlatformSport.Services;

namespace PlatformSport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportController : ControllerBase
    {
        private readonly ISportService _sportService;

        public SportController(ISportService sportService)
        {
            _sportService = sportService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SportDto>>> GetSports()
        {
            var sports = await _sportService.GetAllSportsAsync();
            return Ok(sports);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSport([FromBody] SportDto sportDto)
        {
            var sportId = await _sportService.CreateSportAsync(sportDto);
            return CreatedAtAction(nameof(GetSports), new { id = sportId }, sportDto);
        }

        // SportController.cs
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSport(int id, [FromBody] SportDto sportDto)
        {
            var result = await _sportService.UpdateSportAsync(id, sportDto);
            if (!result)
            {
                return NotFound("Sport not found.");
            }

            return Ok("Sport updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSport(int id)
        {
            var result = await _sportService.DeleteSportAsync(id);
            if (!result)
            {
                return NotFound("Sport not found.");
            }

            return Ok("Sport deleted successfully.");
        }
    }
}
