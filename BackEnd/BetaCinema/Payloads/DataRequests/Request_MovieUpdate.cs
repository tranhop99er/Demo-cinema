namespace BetaCinema.Payloads.DataRequests
{
    public class Request_MovieUpdate : Request_Id
    {
        public DateTime EndTime { get; set; } = DateTime.Now;
        public DateTime PremiereDate { get; set; } = DateTime.Now.AddDays(30);
        public string Description { get; set; }
        public string Director { get; set; }
        public string Image { get; set; }
        public string Language { get; set; }
        public int MovieTypeId { get; set; }
        public string Name { get; set; }
        public int? RateId { get; set; } = 1;
        public string Trailer { get; set; }
    }
}
