using BetaCinema.Entities;

namespace BetaCinema.Payloads.DataResponses
{
    public class DataResponseMovie 
    {
        public int Id { get; set; }
        public int MovieDuration { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime PremiereDate { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string Image { get; set; }
        public string Language { get; set; }
        public int MovieTypedId { get; set; }
        public string Name { get; set; }
        public int? RateId { get; set; }
        public string Trailer { get; set; }
        public string HeroImage { get; set; }
        public double TotalTicketSold { get; set; }
    }
}
