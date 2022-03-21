using BusinessLayer.Services;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer
{
    public class BusinessLayerModule : IModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserService, UserService>();
        }
    }
}