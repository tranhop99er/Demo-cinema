using BetaCinema.Entities;

namespace BetaCinema.Services.Implements
{
    public class BaseService
    {
        public readonly AppDbContext _appDbContext;
        public BaseService()
        {
            _appDbContext = new AppDbContext();
        }
    }
}
