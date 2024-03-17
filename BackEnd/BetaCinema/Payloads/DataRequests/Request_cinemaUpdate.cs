namespace BetaCinema.Payloads.DataRequests
{
    public class Request_cinemaUpdate : Request_Id
    {
        public string Address { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string NameOfCinema { get; set; }

    }
}
