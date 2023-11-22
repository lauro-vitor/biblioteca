using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers.Views
{
    [Route("views/livro")]
    public class LivroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
