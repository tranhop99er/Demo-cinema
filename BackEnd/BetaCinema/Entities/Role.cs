using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class Role : BaseEntity
    {
        public string Code { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<Users>? Users { get; set; }
    }
}
