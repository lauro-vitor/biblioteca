using Biblioteca.Dominio.ViewModel;
using Biblioteca.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers.Api
{
    [Route("api/autor")]
    public class AutorController : ApiBaseController
    {
        private readonly AutorRepositorio _autorRepositorio;

        public AutorController(AutorRepositorio autorRepositorio)
        {
            _autorRepositorio = autorRepositorio;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] AutorParametroViewModel parametro)
        {
            return Ok(_autorRepositorio.Obter(parametro));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await _autorRepositorio.ObterPorId(id));
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] AutorViewModel autorViewModel)
        {
            var resultado = await _autorRepositorio.Inserir(autorViewModel);

            return Created("/autor/" + resultado.IdAutor, resultado);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] AutorViewModel autorViewModel)
        {
            var resultado = await _autorRepositorio.Editar(autorViewModel);

            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] Guid id)
        {
            await _autorRepositorio.Excluir(id);

            return NoContent();
        }
    }
}
