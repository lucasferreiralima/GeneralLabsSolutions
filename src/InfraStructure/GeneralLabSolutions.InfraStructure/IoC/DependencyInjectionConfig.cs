using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.Domain.Notigfications;
using GeneralLabSolutions.Domain.Services.Abstractions;
using GeneralLabSolutions.Domain.Services.Concreted;
using GeneralLabSolutions.InfraStructure.Data;
using GeneralLabSolutions.InfraStructure.Repository;
using GeneralLabSolutions.InfraStructure.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GeneralLabSolutions.InfraStructure.IoC
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddServicesAndDepencencyInjections(this IServiceCollection services)
        {

            services.AddScoped<INotificador, Notificador>(); // Certifique-se de que Notificador é uma implementação válida de INotificador

            // DI Generic Repositories
            services.AddScoped(typeof(IQueryGenericRepository<,>), typeof(QueryGenericRepository<,>));

            // DI Others Repositories
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();


            // DI KanbanTask
            services.AddScoped<IKanbanTaskRepository, KanbanTaskRepository>();
            services.AddScoped<IParticipanteRepository, ParticipanteRepository>();


            // DI Consolidados
            services.AddScoped<IConsolidadoClienteRepository, ConsolidadoClienteRepository>();
            services.AddScoped<IConsolidadoVendedorRepository, ConsolidadoVendedorRepository>();
            services.AddScoped<IConsolidadoFornecedorRepository, ConsolidadoFornecedorRepository>();


            // DI DomainService
            services.AddScoped<IClienteDomainService, ClienteDomainService>();
            services.AddScoped<ICategoriaDomainService, CategoriaDomainService>();

            services.AddScoped<IKanbanTaskDomainService, KanbanTaskDomainService>();
            services.AddScoped<IParticipanteDomainService, ParticipanteDomainService>();

            // Di Others
            services.AddScoped(typeof(IUnitOfWork), typeof(AppDbContext));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



            return services;

        }
    }
}
