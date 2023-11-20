using Biblioteca.Dominio.ViewModel.AlunoContato;
using Biblioteca.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Biblioteca.Apresentacao.Controllers.Api
{
    [Route("api/aluno/contato")]
    public class AlunoContatoController : ApiBaseController
    {
        private readonly AlunoContatoRepositorio _alunoContatoRepositorio;

        public AlunoContatoController(AlunoContatoRepositorio alunoContatoRepositorio)
        {
            _alunoContatoRepositorio = alunoContatoRepositorio;
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(AlunoContatoViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Inserir([FromRoute] Guid id, [FromBody] AlunoContatoViewModel alunoContatoViewModel)
        {
            var resultado = await _alunoContatoRepositorio.Inserir(id, alunoContatoViewModel);

            return Created($"api/aluno/contato/{resultado.IdContato}", resultado);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AlunoContatoViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Editar([FromRoute] Guid id, [FromBody] AlunoContatoViewModel alunoContatoViewModel)
        {
            return Ok(await _alunoContatoRepositorio.Editar(id, alunoContatoViewModel));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Excluir([FromRoute] Guid id, [FromQuery] Guid idContato)
        {
            await _alunoContatoRepositorio.Excluir(id, idContato);

            return NoContent();
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ICollection<AlunoContatoViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Obter([FromRoute] Guid id, [FromQuery] Guid? idContato)
        {
            return Ok(await _alunoContatoRepositorio.Obter(id, idContato));
        }
    }
}
