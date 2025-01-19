namespace PlatformSport.Models.Dto
{
    public class StadiumDto
    {
        public int Id { get; set; }
        public string Name { get; set; }  // Name of the stadium
        public string Location { get; set; }  // Location of the stadium
        public string Description { get; set; }  // Description of the stadium
        public decimal Price { get; set; }  // Price for renting the stadium
        public int SportId { get; set; }  // ID of the sport
    }
}
