// RoomDto.cs
namespace PlatformSport.Models.Dto
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxPlayers { get; set; }
        public DateTime EventStart { get; set; }
        public int SportId { get; set; }
        public int StadiumId { get; set; }  // Add this line
        public string HostUserId { get; set; }
        public List<string> PlayerIds { get; set; }  // List of player IDs in the room
    }
}