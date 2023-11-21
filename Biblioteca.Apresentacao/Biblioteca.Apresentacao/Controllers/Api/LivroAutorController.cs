using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.Autor;
using Biblioteca.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Biblioteca.Apresentacao.Controllers.Api
{
    [Route("api/livro/autor")]
    public class LivroAutorController : ApiBaseController
    {
        private readonly LivroAutorRepositorio _livroAutorRepositorio;

        public LivroAutorController(LivroAutorRepositorio livroAutorRepositorio)
        {
            _livroAutorRepositorio = livroAutorRepositorio;
        }

        [HttpGet("{idLivro}")]
        [ProducesResponseType(typeof(Pagination<AutorViewModel>), (int)HttpStatusCode.OK)]
        public IActionResult ObterAutoresDisponiveis([FromRoute] Guid idLivro, [FromQuery] AutorParametroViewModel parametro)
        {
            return Ok(_livroAutorRepositorio.ObterAutoresDisponiveis(idLivro, parametro));
        }

        [HttpPost("{idLivro}")]
        [ProducesResponseType(typeof(ICollection<LivroAutor>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Inserir([FromRoute] Guid idLivro, [FromBody] ICollection<AutorViewModel>? autoresViewModel)
        {
            await _livroAutorRepositorio.Inserir(idLivro, autoresViewModel);

            return Ok();
        }

        [HttpDelete("{idLivro}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Excluir([FromRoute] Guid idLivro, [FromBody] ICollection<AutorViewModel>? autoresViewModel)
        {
            await _livroAutorRepositorio.Excluir(idLivro, autoresViewModel);

            return NoContent();
        }

    }
}
