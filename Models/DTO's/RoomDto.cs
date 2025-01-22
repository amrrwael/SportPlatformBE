
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
        public SportDto Sport { get; set; } // Include Sport details
        public int StadiumId { get; set; }
        public StadiumDto Stadium { get; set; } // Include Stadium details
        public string HostUserId { get; set; }
        public CityEnum City { get; set; } // This will now be serialized as a string
        public List<string> PlayerIds { get; set; }
    }
}
