using Biblioteca.Dominio.ViewModel.AlunoVM;
using Biblioteca.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Biblioteca.Apresentacao.Controllers.Api
{
    [Route("api/aluno")]
    public class AlunoController : Controller
    {
        private readonly AlunoRepositorio _alunoRepositorio;

        public AlunoController(AlunoRepositorio alunoRepositorio)
        {
            _alunoRepositorio = alunoRepositorio;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AlunoViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Inserir([FromBody] AlunoViewModel alunoViewModel)
        {
            var resultado = await _alunoRepositorio.Inserir(alunoViewModel);

            return Created($"api/aluno/{resultado.IdAluno}", resultado);
        }

        [HttpPut]
        [ProducesResponseType(typeof(AlunoViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Editar([FromBody] AlunoViewModel alunoViewModel)
        {
            return Ok(await _alunoRepositorio.Editar(alunoViewModel));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Excluir([FromRoute] Guid id)
        {
            await _alunoRepositorio.Excluir(id);

            return NoContent();
        }

        [HttpPut("inativar/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Inativar([FromRoute] Guid id)
        {
            await _alunoRepositorio.Inativar(id);

            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AlunoViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterPorId([FromRoute] Guid id)
        {
            return Ok(await _alunoRepositorio.ObterPorId(id));
        }

        [HttpGet]
        [ProducesResponseType(typeof(AlunoViewModel), (int)HttpStatusCode.OK)]
        public IActionResult Obter([FromQuery] AlunoParametroViewModel parametro)
        {
            return Ok(_alunoRepositorio.Obter(parametro));
        }
    }
}
