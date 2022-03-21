using System.Threading;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public interface IDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<UserGroup> UserGroups { get; set; }
        DbSet<UserState> UserStates { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}