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
    }
}
