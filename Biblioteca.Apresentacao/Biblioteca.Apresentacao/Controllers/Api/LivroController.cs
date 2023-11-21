using Biblioteca.Dominio.ViewModel.Livro;
using Biblioteca.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers.Api
{
    [Route("api/livro")]
    public class LivroController : Controller
    {
        private readonly LivroRepositorio _livroRepositorio;

        public LivroController(LivroRepositorio livroRepositorio)
        {
            _livroRepositorio = livroRepositorio;
        }

        [HttpGet]
        public IActionResult Obter([FromQuery] LivroParametroViewModel livroParametroViewModel)
        {
            var keyTitulo = "titulo=";

            if (HttpContext.Request.QueryString.HasValue && (HttpContext.Request.QueryString.Value?.Contains(keyTitulo) ?? false))
            {
                var queryString = HttpContext.Request.QueryString.Value.Replace("?", "").Split("&");

                var titulo = queryString.FirstOrDefault(q => q.StartsWith(keyTitulo));

                if (titulo != null)
                {
                    livroParametroViewModel.Titulo = titulo.Replace(keyTitulo, "");
                }
            }

            return Ok(_livroRepositorio.Obter(livroParametroViewModel));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId([FromRoute] Guid id)
        {
            return Ok(await _livroRepositorio.ObterPorId(id));
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] LivroInputViewModel livroInputViewModel)
        {
            var livro = await _livroRepositorio.Inserir(livroInputViewModel);

            return Created($"/livro/{livro.IdLivro}", livro);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] LivroInputViewModel livroInputViewModel)
        {
            var livro = await _livroRepositorio.Editar(livroInputViewModel);

            return Ok(livro);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] Guid id)
        {
            await _livroRepositorio.Excluir(id);

            return NoContent();
        }
    }
}
