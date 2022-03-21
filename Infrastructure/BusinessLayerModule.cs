using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public class InfrastructureModule : IModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ILockService, LockService>();
        }
    }
}