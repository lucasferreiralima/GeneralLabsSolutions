using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GeneralLabSolutions.Domain.Configurations
{
    public static class MediatRExtensions
    {
        public static IServiceCollection AddMediatRExtencions(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
