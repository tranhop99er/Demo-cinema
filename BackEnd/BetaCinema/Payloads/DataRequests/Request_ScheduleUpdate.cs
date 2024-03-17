namespace BetaCinema.Payloads.DataRequests
{
    public class Request_ScheduleUpdate : Request_Id
    {
        public double Price { get; set; }
        public DateTime StartAt { get; set; } = DateTime.Now.AddHours(24);
        public DateTime EndAt { get; set; } = DateTime.Now.AddHours(26);
        public string? Code { get; set; }
        public int MovieId { get; set; }
        public int RoomId { get; set; }
    }
}
