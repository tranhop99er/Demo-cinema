using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class ConfirmEmails : BaseEntity
    {
        public int UserId { get; set; }
        public Users? User { get; set; }
        public DateTime RequiredDateTime { get; set; }
        public DateTime ExpiredDateTime { get; set; }
        public string ConfirmCode { get; set; }
        public bool IsConfirm { get; set; } = false;
    }
}
