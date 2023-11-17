using Biblioteca.Dominio.ViewModel.Parentesco;
using Biblioteca.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers.Api
{
    [Route("api/parentesco")]
    public class ParentescoController : Controller
    {
        private readonly ParentescoRepositorio _parentescoRepositorio;

        public ParentescoController(ParentescoRepositorio parentescoRepositorio)
        {
            _parentescoRepositorio = parentescoRepositorio;
        }

        [HttpGet]
        public IActionResult Obter([FromQuery] ParentescoParametroViewModel parametro)
        {
            return Ok(_parentescoRepositorio.Obter(parametro));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obter([FromRoute] Guid id)
        {
            return Ok(await _parentescoRepositorio.ObterPorId(id));
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] ParentescoViewModel parentescoViewModel)
        {
            var resultado = await _parentescoRepositorio.Inserir(parentescoViewModel);

            return Created($"api/parentesco/{resultado.IdParentesco}", resultado);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] ParentescoViewModel parentescoViewModel)
        {
            return Ok(await _parentescoRepositorio.Editar(parentescoViewModel));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] Guid id)
        {
            await _parentescoRepositorio.Excluir(id);

            return NoContent();
        }

    }
}
