using BetaCinema.Entities;

namespace BetaCinema.Payloads.DataResponses
{
    public class DataResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }
}
