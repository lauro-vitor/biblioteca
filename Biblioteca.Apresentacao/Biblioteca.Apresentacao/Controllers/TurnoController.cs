using Biblioteca.Dominio.DTO;
using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Repositorio;
using Biblioteca.Dominio.Servico;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Biblioteca.Apresentacao.Controllers
{
    [Route("turno")]
    public class TurnoController : Controller
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
        public ViewResult Index()
        {
            return View("~/Views/Turno/Index.cshtml");
        }

        [Route("views/inserir")]
        public ViewResult Inserir()
        {
            return View("~/Views/Turno/Inserir.cshtml");
        }

        [Route("views/editar/{id}")]
        public ViewResult Editar(Guid id)
        {
            return View("~/Views/Turno/Editar.cshtml");
        }

        [Route("views/excluir/{id}")]
        public ViewResult Excluir(Guid id)
        {
            return View("~/Views/Turno/Excluir.cshtml");
        }

        [HttpGet("obter")]
        [ProducesResponseType(typeof(List<Turno>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Obter()
        {
            try
            {
                var resultado = await _turnoRepositorio.Obter();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                var error = new ErrorResponse(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }

        [HttpGet("obter/{id}")]
        [ProducesResponseType(typeof(Turno), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterPorId(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    var erroResponse = new ErrorResponse();
                    erroResponse.AtribuirErroBadRequest("idTurno", "id do turno é obrigatório");
                    return BadRequest(erroResponse);
                }

                var turno = await _turnoRepositorio.ObterPorId(id.Value);

                if (turno == null)
                {
                    var erroResponse = new ErrorResponse();
                    erroResponse.AtribuirErroNotFound("idTurno", "Turno não encontrado para esse ID");
                    return NotFound(erroResponse);
                }

                return Ok(turno);
            }
            catch (Exception ex)
            {
                var erroResponse = new ErrorResponse(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, erroResponse);
            }
        }
        //ok
        [HttpPost("inserir")]
        [ProducesResponseType(typeof(Turno), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Inserir([FromBody] Turno turno)
        {
            try
            {
                var resultadoValidacao = _turnoServico.ValidarInserir(turno);

                if (resultadoValidacao != null)
                {
                    var errorResponse = new ErrorResponse();
                    errorResponse.AtribuirErroBadRequest(resultadoValidacao);
                    return BadRequest(errorResponse);
                }

                await _turnoRepositorio.Inserir(turno);

                return CreatedAtAction(nameof(ObterPorId), new { id = turno.IdTurno }, turno);
            }
            catch (Exception ex)
            {
                var erroResponse = new ErrorResponse(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, erroResponse);
            }
        }

        [HttpPut("editar")]
        [ProducesResponseType(typeof(Turno), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Editar([FromBody] Turno turno)
        {
            try
            {
                var resultadoValidacao = _turnoServico.ValidarEditar(turno);

                if (resultadoValidacao != null)
                {
                    var errorResponse = new ErrorResponse();
                    errorResponse.AtribuirErroBadRequest(resultadoValidacao);
                    return BadRequest(errorResponse);
                }

                var turnoParaEditar = await _turnoRepositorio.ObterPorId(turno.IdTurno.Value);

                if (turnoParaEditar == null)
                {
                    var errorResponse = new ErrorResponse();
                    errorResponse.AtribuirErroNotFound("idTurno", "Turno não encontrado para esse id");
                    return NotFound(errorResponse);
                }

                await _turnoRepositorio.Editar(turno);

                return Ok(turno);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpDelete("excluir/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Excluir(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    var erroResponse = new ErrorResponse();
                    erroResponse.AtribuirErroBadRequest("idTurno", "id do turno é obrigatório");
                    return BadRequest(erroResponse);
                }

                var turnoParaExcluir = await _turnoRepositorio.ObterPorId(id.Value);

                if (turnoParaExcluir == null)
                {
                    var erroResponse = new ErrorResponse();
                    erroResponse.AtribuirErroNotFound("idTurno", "Turno não encontrado para esse ID");
                    return NotFound(erroResponse);
                }

                await _turnoRepositorio.Excluir(id.Value);

                return NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
