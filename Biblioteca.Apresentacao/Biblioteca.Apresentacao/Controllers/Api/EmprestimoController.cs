using Biblioteca.Dominio.ViewModel.Emprestimo;
using Biblioteca.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Biblioteca.Apresentacao.Controllers.Api
{
    [Route("api/emprestimo")]
    public class EmprestimoController : ApiBaseController
    {
        private readonly EmprestimoRepositorio _emprestimoRepositorio;

        public EmprestimoController(EmprestimoRepositorio emprestimoRepositorio)
        {
            _emprestimoRepositorio = emprestimoRepositorio;
        }

        [HttpGet]
        [ProducesResponseType(typeof(EmprestimoViewModel), (int)HttpStatusCode.OK)]
        public IActionResult Obter([FromQuery] EmprestimoParametroViewModel parametro)
        {
            return Ok(_emprestimoRepositorio.Obter(parametro));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmprestimoDetalheViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterPorId([FromRoute] Guid id)
        {
            return Ok(await _emprestimoRepositorio.ObterPorId(id));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> RealizarEmprestimo([FromQuery] Guid idLivro, [FromQuery] Guid idAluno)
        {
            await _emprestimoRepositorio.RealizarEmprestimo(idLivro, idAluno);

            return Ok(); // tentar fazer um created se tudo der certo
        }

        [HttpPut("devolucao/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> RealizarDevolucao([FromRoute] Guid id)
        {
            await _emprestimoRepositorio.RealizarDevolucao(id);

            return Ok();
        }
    }
}
