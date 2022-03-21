using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public interface IModule
    {
        void Load(IServiceCollection serviceCollection);
    }
}