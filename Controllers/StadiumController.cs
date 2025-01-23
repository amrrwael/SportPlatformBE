// StadiumController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformSport.Database;
using PlatformSport.Models;
using PlatformSport.Models.Dto;
using PlatformSport.Services;

namespace PlatformSport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumController : ControllerBase
    {
        private readonly IStadiumService _stadiumService;
        private readonly ApplicationDbContext _context;

        public StadiumController(IStadiumService stadiumService, ApplicationDbContext context)
        {
            _stadiumService = stadiumService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStadiumAsync([FromBody] StadiumDto stadiumDto)
        {
            var stadium = new Stadium
            {
                Name = stadiumDto.Name,
                Location = stadiumDto.Location,
                Description = stadiumDto.Description,
                Price = stadiumDto.Price,
                SportId = stadiumDto.SportId
            };

            _context.Stadiums.Add(stadium);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStadium), new { id = stadium.Id }, stadium);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStadium(int id)
        {
            var stadium = await _context.Stadiums.FindAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }

            return Ok(stadium);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllStadiums()
        {
            var stadiums = await _stadiumService.GetAllStadiumsAsync();
            return Ok(stadiums);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStadium(int id, [FromBody] StadiumDto stadiumDto)
        {
            var result = await _stadiumService.UpdateStadiumAsync(id, stadiumDto);
            if (!result)
            {
                return NotFound("Stadium not found.");
            }

            return Ok("Stadium updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStadium(int id)
        {
            var result = await _stadiumService.DeleteStadiumAsync(id);
            if (!result)
            {
                return NotFound("Stadium not found.");
            }

            return Ok("Stadium deleted successfully.");
        }
    }
}
