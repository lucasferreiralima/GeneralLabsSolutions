
using FluentValidation.Results;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.Domain.Mensageria;
using GeneralLabSolutions.Domain.Notigfications;
using GeneralLabSolutions.InfraStructure.Repository;
using GeneralLabSolutions.WebAPI.Application.Commands;
using GeneralLabSolutions.WebAPI.Application.Events;
using GeneralLabSolutions.WebAPI.Application.Handlers;
using GeneralLabSolutions.WebAPI.Services;
using GeneralLabSolutions.WebAPI.Workers;
using MediatR;

namespace GeneralLabSolutions.WebAPI.Configurations
{
    public static class ApiDIConfig
    {
        public static IServiceCollection RegisterApiServices(this IServiceCollection services)
        {
            #region: Cliente CQRS, Mensageria, Hosted e Notificações

            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, RegistrarClienteCommandHandler>();


            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            // DI Mensageria
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Paganition in WebApi
            services.AddScoped<IPaginationService, PaginationService>();

            // Registro do nosso serviço em segundo plano
            services.AddHostedService<ContasPagarReceberWorker>();
            services.AddHostedService<PagamentoRecebidoWorker>();

            return services;


            #endregion

        }
    }
}
