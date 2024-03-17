using BetaCinema.Entities;

namespace BetaCinema.Payloads.DataRequests
{
    public class Request_Seat
    {
        public int Number { get; set; }
        public int SeatStatusId { get; set; }
        public string Line { get; set; }
        public int RoomId { get; set; }
        public int SeatTypeId { get; set; }
    }
}
