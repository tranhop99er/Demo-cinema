namespace BetaCinema.Payloads.DataRequests
{
    public class Request_TicketUpdate : Request_Id
    {
        public string Code { get; set; } = "abc_Cupper-Seat";
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
        public double PriceTicket { get; set; }
    }
}
