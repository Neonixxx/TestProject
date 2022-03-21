using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddModule(this IServiceCollection serviceCollection, IModule module)
        {
            module.Load(serviceCollection);
            return serviceCollection;
        }
        
        public static bool TryLock(this SemaphoreSlim semaphoreSlim)
        {
            if (semaphoreSlim.CurrentCount < 1)
            {
                return false;
            }

            semaphoreSlim.Wait();
            return true;
        }
    }
}