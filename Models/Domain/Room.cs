// Room.cs
using System;
using System.Collections.Generic;

namespace PlatformSport.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }  // Name of the room
        public string Description { get; set; }  // Description of the room
        public int MaxPlayers { get; set; }  // Maximum number of players allowed
        public DateTime EventStart { get; set; }  // When the event will start
        public int SportId { get; set; }  // ID of the sport
        public Sport Sport { get; set; }  // Navigation property to Sport
        public string HostUserId { get; set; }  // ID of the user who created the room (host)
        public User HostUser { get; set; }  // Navigation property to User (host)
        public ICollection<User> Players { get; set; } = new List<User>();  // List of players in the room
    }
}