using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class Schedules : BaseEntity
    {
        public double Price { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string? Code { get; set; }
        public int MovieId { get; set; }
        public Movies? Movie { get; set; }
        public string Name { get; set;}
        public int RoomId { get; set;}
        public Room? Room { get; set; }
        public bool? IsActive { get; set; } = true;
        public IEnumerable<Tickets>? Tickets { get; set; }
    }
}
