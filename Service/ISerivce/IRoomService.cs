using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformSport.Database;
using PlatformSport.Models;
using PlatformSport.Models.Dto;

namespace PlatformSport.Services
{
    public interface IRoomService
    {
        Task<RoomDto> CreateRoomAsync(CreateRoomDto dto, string hostUserId);
        Task<RoomDto> GetRoomByIdAsync(int roomId);
        Task<bool> JoinRoomAsync(int roomId, string userId);
        Task<bool> LeaveRoomAsync(int roomId, string userId);
    }
}