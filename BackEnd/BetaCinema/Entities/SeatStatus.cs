namespace BetaCinema.Entities
{
    public class SeatStatus : BaseEntity
    {
        public string Code { get; set; }
        public string NameStatus { get; set; }
        public IEnumerable<Seat>? Seat { get; set; } 
    }
}
