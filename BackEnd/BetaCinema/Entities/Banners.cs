using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class Banners : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
    }
}
