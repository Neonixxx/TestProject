using DataAccess.Repositories;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public class DataAccessModule : IModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserGroupRepository, UserGroupRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IUserStateRepository, UserStateRepository>();
        }
    }
}