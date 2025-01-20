using System.Collections.Generic;

namespace PlatformSport.Models
{
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }  // Name of the sport (e.g., Football, Basketball)

        // A sport can have many stadiums
        public ICollection<Stadium> Stadiums { get; set; }
    }
}