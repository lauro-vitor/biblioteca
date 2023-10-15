using Biblioteca.Dominio.ViewModel;
using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Repositorio;
using Biblioteca.Dominio.Servico;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers
{
	[Route("turno")]
	public class TurnoController : ApiBaseController
	{
		private readonly ITurnoServico _turnoServico;
		private readonly ITurnoRepositorio _turnoRepositorio;
		public TurnoController(ITurnoRepositorio turnoRepositorio,
							   ITurnoServico turnoServico)
		{
			_turnoRepositorio = turnoRepositorio;
			_turnoServico = turnoServico;
		}

		[Route("views/index")]
		public ViewResult IndexVW()
		{
			var view = View("~/Views/Turno/Index.cshtml");

			view.ViewData["title"] = "Turno";

			return view;
		}

		[Route("views/inserir")]
		public ViewResult InserirVW()
		{
			var view = View("~/Views/Turno/Inserir.cshtml");

			view.ViewData["title"] = "Inserir turno";

			return view;
		}

		[Route("views/editar/{id}")]
		public ViewResult EditarVW(Guid id)
		{
			var view = View("~/Views/Turno/Editar.cshtml");

			view.ViewData["title"] = "Editar turno";

			return view;
		}

		[Route("views/excluir/{id}")]
		public ViewResult ExcluirVW(Guid id)
		{
			var view = View("~/Views/Turno/Excluir.cshtml");

			view.ViewData["title"] = "Excluir turno";

			return view;
		}

		[HttpGet("obter")]
		[ProducesResponseType(typeof(List<Turno>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ErroViewModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Obter()
		{
			var resultado = await _turnoRepositorio.Obter();

			return Ok(resultado);
		}

		[HttpGet("obter/{id}")]
		public async Task<IActionResult> ObterPorId(Guid? id)
		{
			if (id == null)
			{
				return BadRequestResultIdInvalid();
			}

			var turno = await _turnoRepositorio.ObterPorId(id.Value);

			if (turno == null)
			{
				return NotFoundResult();
			}

			return Ok(turno);
		}

		[HttpPost("inserir")]
		public async Task<IActionResult> Inserir([FromBody] Turno turnoParaInserir)
		{
			var turnosCadastrados = await _turnoRepositorio.Obter();

			var resultadoValidacao = _turnoServico.ValidarInserirTurno(turnoParaInserir, turnosCadastrados);

			if (resultadoValidacao != null)
			{
				return BadRequest(resultadoValidacao);
			}

			await _turnoRepositorio.Inserir(turnoParaInserir);

			return CreatedAtAction(nameof(ObterPorId), new { id = turnoParaInserir.IdTurno }, turnoParaInserir);
		}

		[HttpPut("editar/{id}")]
		public async Task<IActionResult> Editar([FromRoute] Guid? id, [FromBody] Turno turno)
		{
			if (id == null)
			{
				return BadRequestResultIdInvalid();
			}

			var turnosCadastrados = await _turnoRepositorio.Obter();

			var resultadoTurnoNaoExiste = _turnoServico.ValidarTurnoNaoExiste(id.Value, turnosCadastrados);

			if (resultadoTurnoNaoExiste != null)
			{
				return NotFound(resultadoTurnoNaoExiste);
			}

			var resultadoValidacao = _turnoServico.ValidarEditarTurno(turno, turnosCadastrados);

			if (resultadoValidacao != null)
			{
				return BadRequest(resultadoValidacao);
			}

			await _turnoRepositorio.Editar(turno);

			return Ok(turno);
		}

		[HttpDelete("excluir/{id}")]
		public async Task<IActionResult> Excluir(Guid? id)
		{
			if (id == null)
			{
				return BadRequestResultIdInvalid();
			}

			var turnosCadastrados = await _turnoRepositorio.Obter();

			var resultadoTurnoNaoExiste = _turnoServico.ValidarTurnoNaoExiste(id.Value, turnosCadastrados);

			if (resultadoTurnoNaoExiste != null)
			{
				return NotFound(resultadoTurnoNaoExiste);
			}

			await _turnoRepositorio.Excluir(id.Value);

			return NoContent();
		}

	}
}
