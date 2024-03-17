using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class Tickets : BaseEntity
    {
        public string Code { get; set;}
        public int ScheduleId { get; set;}
        public Schedules? Schedule { get; set;}
        public int SeatId { get; set;}
        public Seat? Seat { get; set;}
        public bool? IsActive { get; set; } = true;
        public double PriceTicket { get; set;}
        public IEnumerable<BillTickets>? BillTickets { get; set;}
    }
}
