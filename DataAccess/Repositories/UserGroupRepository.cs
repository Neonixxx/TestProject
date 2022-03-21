using System.Linq;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public interface IUserGroupRepository
    {
        Task<int> GetDefaultId();
    }
    
    internal class UserGroupRepository : IUserGroupRepository
    {
        private readonly IDbContext _context;

        public UserGroupRepository(IDbContext context)
        {
            _context = context;
        }

        public Task<int> GetDefaultId()
        {
            return _context.UserGroups.Where(x => x.Code == UserGroupCode.User)
                .Select(x => x.UserGroupId)
                .FirstAsync();
        }
    }
}