namespace BetaCinema.Entities
{
    public class SeatType : BaseEntity
    {
        public string NameType { get; set;}
        public IEnumerable<Seat>? Seat { get; set; }
    }
}
