using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class RefreshTokens : BaseEntity
    {
        public string Token { get; set; }
        public DateTime ExpiredTime { get; set; }
        public int UserId { get; set; }
        public Users? User { get; set; }
    }
}
