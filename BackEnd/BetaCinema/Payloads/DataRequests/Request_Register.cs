using BetaCinema.Entities;
using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Payloads.DataRequests
{
    public class Request_Register
    {
        [Required]
        [MinLength(4)]
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        [Required]
        [MinLength(9)]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        
        //public RankCustomers? RankCustomer { get; set; }
        //public UserStatus? UserStatus { get; set; }
    }
}
