﻿using PlatformSport.Models;
using PlatformSport.Models.Dto;
using Microsoft.EntityFrameworkCore;
using PlatformSport.Database;

namespace PlatformSport.Services
{
    public class StadiumService : IStadiumService
    {
        private readonly ApplicationDbContext _context;

        public StadiumService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StadiumDto>> GetAllStadiumsAsync(int sportId)
        {
            var stadiums = await _context.Stadiums
                .Where(s => s.SportId == sportId)
                .ToListAsync();

            return stadiums.Select(s => new StadiumDto
            {
                Id = s.Id,
                Name = s.Name,
                Location = s.Location,
                Description = s.Description,
                Price = s.Price,
                SportId = s.SportId
            }).ToList();
        }

        public async Task<StadiumDto> GetStadiumByIdAsync(int stadiumId)
        {
            var stadium = await _context.Stadiums
                .FirstOrDefaultAsync(s => s.Id == stadiumId);

            if (stadium == null) return null;

            return new StadiumDto
            {
                Id = stadium.Id,
                Name = stadium.Name,
                Location = stadium.Location,
                Description = stadium.Description,
                Price = stadium.Price,
                SportId = stadium.SportId
            };
        }

        public async Task<int> CreateStadiumAsync(StadiumDto stadiumDto)
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

            return stadium.Id;
        }
    }
}
