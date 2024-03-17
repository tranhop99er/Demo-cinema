using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class RankCustomers : BaseEntity
    {
        public int Point { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
        public IEnumerable<Users>? Users { get; set; }
        public IEnumerable<Promotions>? Promotions { get; set; }
    }
}
