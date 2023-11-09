using Biblioteca.Dominio.ViewModel;
using Biblioteca.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers
{
    [Route("livro")]
    public class LivroController : Controller
    {
        private readonly LivroRepositorio _livroRepositorio;

        public LivroController(LivroRepositorio livroRepositorio)
        {
            _livroRepositorio = livroRepositorio;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _livroRepositorio.Obter());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await _livroRepositorio.ObterPorId(id));
        }

        [HttpPost]
        public async Task<IActionResult>Post([FromBody]LivroViewModel livroViewModel)
        {
            var livro = await _livroRepositorio.Inserir(livroViewModel);

            return Created($"/livro/{livro.IdLivro}", livro);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]LivroViewModel livroViewModel)
        {
            await _livroRepositorio.Editar(livroViewModel);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _livroRepositorio.Excluir(id);

            return NoContent();
        }

    }
}
