using Microsoft.EntityFrameworkCore;
using PlatformSport.Database;
using PlatformSport.Models;
using PlatformSport.Models.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformSport.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        // RoomService.cs
        public async Task<RoomDto> CreateRoomAsync(CreateRoomDto dto, string hostUserId)
        {
            // Check if the Stadium exists
            

            var room = new Room
            {
                Name = dto.Name,
                Description = dto.Description,
                MaxPlayers = dto.MaxPlayers,
                EventStart = dto.EventStart,
                SportId = dto.SportId,
                HostUserId = hostUserId
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                MaxPlayers = room.MaxPlayers,
                EventStart = room.EventStart,
                SportId = room.SportId,
                HostUserId = room.HostUserId,
                PlayerIds = new List<string>()
            };
        }

        public async Task<RoomDto> GetRoomByIdAsync(int roomId)
        {
            var room = await _context.Rooms
                .Include(r => r.Players)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null) return null;

            return new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                MaxPlayers = room.MaxPlayers,
                EventStart = room.EventStart,
                SportId = room.SportId,
                HostUserId = room.HostUserId,
                PlayerIds = room.Players.Select(p => p.Id).ToList()
            };
        }

        public async Task<bool> JoinRoomAsync(int roomId, string userId)
        {
            var room = await _context.Rooms
                .Include(r => r.Players)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null || room.Players.Count >= room.MaxPlayers)
                return false;

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            room.Players.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> LeaveRoomAsync(int roomId, string userId)
        {
            var room = await _context.Rooms
                .Include(r => r.Players)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null) return false;

            var user = room.Players.FirstOrDefault(p => p.Id == userId);
            if (user == null) return false;

            room.Players.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}