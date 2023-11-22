using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.ViewModel;
using Biblioteca.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers
{
	[Route("turma")]
	public class TurmaController : ApiBaseController
	{
		private readonly TurmaRepositorio _turmaRepositorio;

		public TurmaController(TurmaRepositorio turmaRepositorio)
		{
			_turmaRepositorio = turmaRepositorio;
		}

		[HttpGet]
		[Route("obter")]
		public IActionResult Obter(
			string? idTurno = null,
			string? nome = null,
			int? periodo = null,
			string? sigla = null,
			string? ordenacaoCampo = null,
			string? ordenacaoDirecao = null,
			int? pageNumber = null,
			int? pageSize = null
		 )
		{

			var turmas = _turmaRepositorio.Obter(
				idTurno,
				nome,
				periodo,
				sigla,
				ordenacaoCampo,
				ordenacaoDirecao,
				pageNumber,
				pageSize
			);

			return Ok(turmas);
		}

		[HttpGet("obterPorId/{id:Guid}")]
		public async Task<IActionResult> ObterPorId(Guid id)
		{
			var turma = await _turmaRepositorio.ObterPorId(id);

			if (turma == null)
			{
				return NotFoundResult();
			}

			return Ok(turma);
		}

		[HttpPost("inserir")]
		public async Task<IActionResult> Inserir([FromBody] TurmaViewModel turma)
		{

			if (turma == null)
			{
				return BadRequestResult();
			}

			var turmaParaInserir = new Turma(
				turma.IdTurma,
				turma.IdTurno,
				turma.Nome,
				turma.Periodo,
				turma.Sigla
			);

			await _turmaRepositorio.Inserir(turmaParaInserir);

			return Created($"/turma/obterPorId/{turmaParaInserir.IdTurma}", turma);
		}

		[HttpPut("editar")]
		public async Task<IActionResult> Editar([FromBody] TurmaViewModel turma)
		{

			if (turma == null)
			{
				return BadRequestResult();
			}

			var turmaParaEditar = new Turma(
					turma.IdTurma,
					turma.IdTurno,
					turma.Nome,
					turma.Periodo,
					turma.Sigla
				);

			await _turmaRepositorio.Editar(turmaParaEditar);

			return Ok(turma);
		}

		[HttpDelete]
		[Route("excluir/{id:Guid}")]
		public async Task<IActionResult> Excluir([FromRoute] Guid id)
		{
			if (id == Guid.Empty)
			{
				return BadRequestResultIdInvalid();
			}

			var turma = await _turmaRepositorio.ObterPorId(id);

			if (turma == null)
			{
				return NotFoundResult();
			}

			await _turmaRepositorio.Excluir(id);

			return NoContent();
		}

		#region VIEWS

		[Route("views/index")]
		[HttpGet]
		public ViewResult IndexVW()
		{
			return View("~/Views/Turma/Index.cshtml");
		}

		[Route("views/inserir")]
        [HttpGet]
        public ViewResult InserirVW()
		{
			return View("~/Views/Turma/Inserir.cshtml");
		}

		[Route("views/editar/{id}")]
        [HttpGet]
        public ViewResult EditarVW(Guid id)
		{
			return View();
		}

		[Route("views/excluir/{id}")]
        [HttpGet]
        public ViewResult ExcluirVW(Guid id)
		{
			return View();
		}
		#endregion

	}
}
