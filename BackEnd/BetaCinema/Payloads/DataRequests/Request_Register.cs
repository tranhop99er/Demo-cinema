using BetaCinema.Entities;

namespace BetaCinema.Payloads.DataRequests
{
    public class Request_Register
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        //public RankCustomers? RankCustomer { get; set; }
        //public UserStatus? UserStatus { get; set; }
    }
}
