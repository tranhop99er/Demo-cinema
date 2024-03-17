namespace BetaCinema.Payloads.DataRequests
{
    public class Request_RoomUpdate : Request_Id
    {
        public int Capacity { get; set; }
        public string Description { get; set; }
        public int CinemaId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
