using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class Users : BaseEntity
    {
        public int? Point { get; set; } = 0;
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int? RankCustomerId { get; set; }
        public RankCustomers? RankCustomer { get; set; }
        public int UserStatusId { get; set; } = 2;
        public UserStatus? UserStatus { get; set; }
        public bool? IsActive { get; set; } = true;
        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public IEnumerable<RefreshTokens>? RefreshTokens { get; set; }
        public IEnumerable<Bill>? Bill { get; set; }
        public IEnumerable<ConfirmEmails>? ConfirmEmails { get; set; }
    }
}
