using GeneralLabSolutions.WebAPI.Application.Events;
using MediatR;

namespace GeneralLabSolutions.WebAPI.Application.Handlers
{
    public class ClienteEventHandler : INotificationHandler<ClienteRegistradoEvent>
    {
        public Task Handle(ClienteRegistradoEvent notification, CancellationToken cancellationToken)
        {
            // Enviar evento de confirmação;
            // Enviar emails para os interessados;
            // Fazer outras coisas relevantes;
            return Task.CompletedTask;
        }
    }
}
