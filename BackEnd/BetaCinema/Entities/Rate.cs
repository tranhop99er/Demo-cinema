using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class Rate : BaseEntity
    {
        public string Description { get; set; }
        public string Code { get; set; }
        public IEnumerable<Movies>? Movies { get; set; }
    }
}
