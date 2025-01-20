using PlatformSport.Models;
using PlatformSport.Models.Dto;
using Microsoft.EntityFrameworkCore;
using PlatformSport.Database;

namespace PlatformSport.Services
{
    public class SportService : ISportService
    {
        private readonly ApplicationDbContext _context;

        public SportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SportDto>> GetAllSportsAsync()
        {
            var sports = await _context.Sports.ToListAsync();
            return sports.Select(s => new SportDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }

        public async Task<SportDto> GetSportByIdAsync(int sportId)
        {
            var sport = await _context.Sports
                .Include(s => s.Stadiums)  // Include related stadiums
                .FirstOrDefaultAsync(s => s.Id == sportId);

            if (sport == null) return null;

            return new SportDto
            {
                Id = sport.Id,
                Name = sport.Name
            };
        }

        public async Task<int> CreateSportAsync(SportDto sportDto)
        {
            var sport = new Sport
            {
                Name = sportDto.Name
            };

            _context.Sports.Add(sport);
            await _context.SaveChangesAsync();

            return sport.Id;
        }
    }
}
