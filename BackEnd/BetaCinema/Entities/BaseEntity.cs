using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
