using Microsoft.AspNetCore.Mvc;

namespace VelzonModerna.Controllers
{
    // Todo: Implementar as Actions e Views
    public class HistoricoDePedidoController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }
    }
}
