using BetaCinema.Entities;

namespace BetaCinema.Payloads.DataRequests
{
    public class Request_Ticket
    {
        public string Code { get; set; } = "abc_Cupper-Seat";
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
        public double PriceTicket { get; set; }
    }
}
