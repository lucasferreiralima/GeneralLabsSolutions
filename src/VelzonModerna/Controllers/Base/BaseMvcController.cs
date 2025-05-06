using GeneralLabSolutions.Domain.Notigfications;
using Microsoft.AspNetCore.Mvc;
using velzon.Models;

namespace VelzonModerna.Controllers.Base
{
    public abstract class BaseMvcController : Controller
    {
        private readonly INotificador _notificador;

        protected BaseMvcController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

    }
}
