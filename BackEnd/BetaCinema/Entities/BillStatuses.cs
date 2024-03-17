using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class BillStatuses : BaseEntity
    {
        public string Name { get; set; }
        public IEnumerable<Bill> Bill { get; set; }
    }
}
