using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsWithLoginAsync(string login);
        Task AddAsync(User user);
        Task<User> GetAsync(int userId);
        Task<List<User>> GetAllAsync();
        Task<string> GetLoginAsync(int userId);
        Task DeleteAsync(int userId, int deletedUserStatusId);
        Task<User> GetByLoginAsync(string login);
    }
    
    internal class UserRepository : IUserRepository
    {
        private readonly IDbContext _context;

        public UserRepository(IDbContext context)
        {
            _context = context;
        }

        public Task<bool> ExistsWithLoginAsync(string login)
        {
            return _context.Users.AnyAsync(x => x.Login == login && x.UserState.Code == UserStateCode.Active);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public Task<User> GetAsync(int userId)
        {
            return _context.Users.AsNoTracking()
                .Include(x => x.UserGroup)
                .Include(x => x.UserState)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.UserState.Code == UserStateCode.Active);
        }

        public Task<List<User>> GetAllAsync()
        {
            return _context.Users.AsNoTracking()
                .Include(x => x.UserGroup)
                .Include(x => x.UserState)
                .Where(x => x.UserState.Code == UserStateCode.Active)
                .ToListAsync();
        }

        public Task<string> GetLoginAsync(int userId)
        {
            return _context.Users.AsNoTracking()
                .Where(x => x.UserId == userId && x.UserState.Code == UserStateCode.Active)
                .Select(x => x.Login)
                .FirstOrDefaultAsync();
        }

        public Task DeleteAsync(int userId, int deletedUserStatusId)
        {
            return _context.Users.Where(x => x.UserId == userId).UpdateAsync(user => new User { UserStateId = deletedUserStatusId });
        }

        public Task<User> GetByLoginAsync(string login)
        {
            return _context.Users.AsNoTracking()
                .Include(x => x.UserGroup)
                .Where(x => x.Login == login && x.UserState.Code == UserStateCode.Active)
                .FirstOrDefaultAsync();
        }
    }
}