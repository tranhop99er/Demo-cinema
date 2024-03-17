using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class MovieType : BaseEntity
    {
        public string MovieTypeName { get; set; }
        public bool? IsActive { get; set; } = true;
        public IEnumerable<Movies> Movies { get; set; }
    }
}
