using Biblioteca.Dominio.ViewModel.Genero;
using Biblioteca.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers.Api
{
    [Route("api/livro/genero")]
    public class LivroGeneroController : Controller
    {
        private readonly LivroGeneroRepositorio _livroGeneroRepositorio;

        public LivroGeneroController(LivroGeneroRepositorio livroGeneroRepositorio)
        {
            _livroGeneroRepositorio = livroGeneroRepositorio;
        }

        [HttpGet("{idLivro}")]
        public IActionResult ObterGenerosDisponiveis([FromRoute] Guid idLivro, [FromQuery] GeneroParametroViewModel parametro)
        {
            return Ok(_livroGeneroRepositorio.ObterGenerosDisponiveis(idLivro, parametro));
        }

        [HttpPost("{idLivro}")]
        public async Task<IActionResult> Inserir([FromRoute] Guid idLivro, [FromBody] ICollection<GeneroViewModel> generosViewModel)
        {
            await _livroGeneroRepositorio.Inserir(idLivro, generosViewModel);

            return Ok();
        }

        [HttpDelete("{idLivro}")]
        public async Task<IActionResult> Excluir([FromRoute] Guid idLivro, [FromBody] ICollection<GeneroViewModel> generos)
        {
            await _livroGeneroRepositorio.Excluir(idLivro, generos);

            return Ok();
        }

    }
}
