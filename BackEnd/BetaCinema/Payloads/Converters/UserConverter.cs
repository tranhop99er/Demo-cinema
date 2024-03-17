 using BetaCinema.Entities;
using BetaCinema.Payloads.DataResponses;
using Microsoft.EntityFrameworkCore;

namespace BetaCinema.Payloads.Converters
{
    public class UserConverter
    {
        private readonly AppDbContext _appDbContext;
        public UserConverter()
        {
            _appDbContext = new AppDbContext();
        }
        public DataResponse EntityToDTO(Users users)
        {
            var userItem = _appDbContext.users.Include(x => x.Role).AsNoTracking().SingleOrDefault(x => x.Id == users.Id);
            return new DataResponse
            {
                Username = users.Username,
                Email = users.Email,
                Name = users.Name,
                PhoneNumber = users.PhoneNumber,
                Password = users.Password,
                RoleName = userItem.Role.RoleName
            };
        }
    }
}
