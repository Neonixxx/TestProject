using System.Linq;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public interface IUserStateRepository
    {
        Task<int> GetActiveIdAsync();
        Task<int> GetBlockedIdAsync();
    }
    
    internal class UserStateRepository : IUserStateRepository
    {
        private readonly IDbContext _context;

        public UserStateRepository(IDbContext context)
        {
            _context = context;
        }

        public Task<int> GetActiveIdAsync()
        {
            return _context.UserStates.Where(x => x.Code == UserStateCode.Active)
                .Select(x => x.UserStateId)
                .FirstAsync();
        }

        public Task<int> GetBlockedIdAsync()
        {
            return _context.UserStates.Where(x => x.Code == UserStateCode.Blocked)
                .Select(x => x.UserStateId)
                .FirstAsync();
        }
    }
}