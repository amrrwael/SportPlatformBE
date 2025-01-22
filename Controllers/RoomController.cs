// RoomController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlatformSport.Models;
using PlatformSport.Models.Dto;
using PlatformSport.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlatformSport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRooms(
    [FromQuery] string sortBy = null, // Optional: "price" or "players"
    [FromQuery] string city = null,  // Optional: Filter by city
    [FromQuery] int? stadiumId = null) // Optional: Filter by stadiumId
        {
            // Validate city filter
            if (!string.IsNullOrEmpty(city) && !Enum.TryParse(city, out CityEnum _))
            {
                return BadRequest("Invalid city. Valid options are Tomsk and Novosibirsk.");
            }

            var rooms = await _roomService.GetAllRoomsAsync(sortBy, city, stadiumId);
            return Ok(rooms);
        }
        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var room = await _roomService.CreateRoomAsync(dto, userId);

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
                return NotFound();

            return Ok(room);
        }

        [HttpPost("Join/{roomId}")]
        [Authorize]
        public async Task<IActionResult> JoinRoom(int roomId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _roomService.JoinRoomAsync(roomId, userId);

            if (!result)
                return BadRequest("Room is full or does not exist.");

            return Ok();
        }

        [HttpPost("Leave/{roomId}")]
        [Authorize]
        public async Task<IActionResult> LeaveRoom(int roomId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _roomService.LeaveRoomAsync(roomId, userId);

            if (!result)
                return BadRequest("User is not in the room or room does not exist.");

            return Ok();
        }
    }
}
