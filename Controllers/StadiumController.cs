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
        private readonly ApplicationDbContext _context; // Inject the DbContext

        public StadiumController(IStadiumService stadiumService, ApplicationDbContext context)
        {
            _stadiumService = stadiumService;
            _context = context; // Assign DbContext
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

    }
}
