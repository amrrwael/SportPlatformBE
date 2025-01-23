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
        public async Task<List<RoomDto>> GetAllRoomsAsync(string sortBy, string cityFilter, int? stadiumIdFilter)
        {
            // Start with the base query
            var query = _context.Rooms
                .Include(r => r.Players) // Include Players
                .Include(r => r.Stadium) // Include Stadium
                .Include(r => r.Sport)   // Include Sport
                .Include(r => r.HostUser) // Include HostUser to get the host's name
                .AsQueryable();

            // Apply city filter
            if (!string.IsNullOrEmpty(cityFilter))
            {
                // Parse the cityFilter string into the CityEnum
                if (Enum.TryParse(cityFilter, out CityEnum cityEnum))
                {
                    query = query.Where(r => r.City == cityEnum); // Compare directly with the enum value
                }
                else
                {
                    // If the cityFilter is invalid, return an empty list or handle the error as needed
                    return new List<RoomDto>();
                }
            }

            // Apply stadium filter
            if (stadiumIdFilter.HasValue)
            {
                query = query.Where(r => r.StadiumId == stadiumIdFilter.Value);
            }

            // Apply sorting
            switch (sortBy?.ToLower())
            {
                case "price":
                    query = query.OrderBy(r => r.Stadium.Price); // Sort by stadium price
                    break;
                case "players":
                    query = query.OrderBy(r => r.Players.Count); // Sort by number of players
                    break;
                default:
                    query = query.OrderBy(r => r.EventStart); // Default sorting by event start time
                    break;
            }

            // Execute the query and map to RoomDto
            var rooms = await query
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    MaxPlayers = r.MaxPlayers,
                    EventStart = r.EventStart,
                    SportId = r.SportId,
                    Sport = new SportDto // Map Sport details
                    {
                        Id = r.Sport.Id,
                        Name = r.Sport.Name
                    },
                    StadiumId = r.StadiumId,
                    Stadium = new StadiumDto // Map Stadium details
                    {
                        Id = r.Stadium.Id,
                        Name = r.Stadium.Name,
                        Location = r.Stadium.Location,
                        City = r.Stadium.City,
                        Description = r.Stadium.Description,
                        Price = r.Stadium.Price
                    },
                    HostUserId = r.HostUserId,
                    HostUserName = r.HostUser.FullName, // Add host's name
                    City = r.City,
                    PlayerIds = r.Players.Select(p => p.Id).ToList()
                })
                .ToListAsync();

            return rooms;
        }


        public async Task<RoomDto> CreateRoomAsync(CreateRoomDto dto, string hostUserId)
        {
            var room = new Room
            {
                Name = dto.Name,
                Description = dto.Description,
                MaxPlayers = dto.MaxPlayers,
                EventStart = dto.EventStart,
                SportId = dto.SportId,
                StadiumId = dto.StadiumId,  // Add this line
                HostUserId = hostUserId,
                City = dto.City
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
                StadiumId = room.StadiumId,  // Add this line
                HostUserId = room.HostUserId,
                City = room.City,
                PlayerIds = new List<string>()
            };
        }

        public async Task<RoomDto> GetRoomByIdAsync(int roomId)
        {
            var room = await _context.Rooms
                .Include(r => r.Players)
                .Include(r => r.HostUser) // Include HostUser to get the host's name
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
                HostUserName = room.HostUser.FullName, // Add host's name
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
        public async Task<bool> DeleteRoomAsync(int roomId, string hostUserId)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == roomId && r.HostUserId == hostUserId);

            if (room == null)
            {
                return false; // Room not found or user is not the host
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
