using aspnetcore_mementable.MementoExtension.Interfaces;
using MementoExtension.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace aspnetcore_mementable.MementoExtension.Services
{
    public static class RegisterServices
    {
        public static void RegisterState<T>(this IServiceCollection services)
            where T : class, Mementable
        {
            services.AddScoped<StateManager<T>>();
        }

        public static void RegisterStateDbContext<T>(this IServiceCollection services)
            where T : class, StateDbContext
        {
            services.AddScoped<StateDbContext, T>();
        }
    }
}