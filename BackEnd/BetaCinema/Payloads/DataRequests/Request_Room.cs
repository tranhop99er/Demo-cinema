using BetaCinema.Entities;

namespace BetaCinema.Payloads.DataRequests
{
    public class Request_Room
    {
        public int Capacity { get; set; }
        public string Description { get; set; }
        public int CinemaId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }
}
