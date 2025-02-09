
namespace PlatformSport.Models.Dto
{
    public class CreateRoomDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxPlayers { get; set; }
        public DateTime EventStart { get; set; }
        public int SportId { get; set; }
        public int StadiumId { get; set; }  // Foreign key to Stadium
        public CityEnum City { get; set; }  // City where the room is located
    }
}
