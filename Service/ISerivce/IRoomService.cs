using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformSport.Models.Dto;

namespace PlatformSport.Services
{
    public interface IRoomService
    {
        Task<RoomDto> CreateRoomAsync(CreateRoomDto dto, string hostUserId);
        Task<RoomDto> GetRoomByIdAsync(int roomId);
        Task<bool> JoinRoomAsync(int roomId, string userId);
        Task<bool> LeaveRoomAsync(int roomId, string userId);
        Task<List<RoomDto>> GetAllRoomsAsync(string sortBy, string cityFilter, int? stadiumIdFilter);
        Task<bool> DeleteRoomAsync(int roomId, string hostUserId); // Add this method
    }
}
