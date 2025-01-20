namespace PlatformSport.Models
{
    public class Stadium
    {
        public int Id { get; set; }
        public string Name { get; set; }  // Name of the stadium
        public string Location { get; set; }  // Location of the stadium
        public string Description { get; set; }  // A description of the stadium
        public decimal Price { get; set; }  // The price for renting the stadium (per hour or other metric)

        // Foreign key to the Sport model
        public int SportId { get; set; }
        public Sport Sport { get; set; }
    }
}