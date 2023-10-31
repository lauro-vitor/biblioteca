using Biblioteca.Dominio.ViewModel;
using Biblioteca.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers
{
    [Route("genero")]
    public class GeneroController : Controller
    {
        private readonly GeneroRepositorio _generoRepositorio;

        public GeneroController(GeneroRepositorio generoRepositorio)
        {
            _generoRepositorio = generoRepositorio;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _generoRepositorio.Obter());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await _generoRepositorio.ObterPorId(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GeneroViewModel generoViewModel)
        {
            var genero = await _generoRepositorio.Inserir(generoViewModel);

            return Created($"/genero/{genero.IdGenero}", genero);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GeneroViewModel generoViewModel)
        {
            return Ok(await _generoRepositorio.Editar(generoViewModel));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _generoRepositorio.Excluir(id);

            return NoContent();
        }
    }
}
