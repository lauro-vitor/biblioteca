using Biblioteca.Dominio.DTO;
using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Repositorio;
using Biblioteca.Dominio.Servico;
using Microsoft.AspNetCore.Mvc;

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
            var view = View("~/Views/Turno/Index.cshtml");

            view.ViewData["title"] = "Turno";

            return view;
        }

        [Route("views/inserir")]
        public ViewResult Inserir()
        {
            var view = View("~/Views/Turno/Inserir.cshtml");

            view.ViewData["title"] = "Inserir turno";

            return view;
        }

        [Route("views/editar/{id}")]
        public ViewResult Editar(Guid id)
        {
            var view = View("~/Views/Turno/Editar.cshtml");

            view.ViewData["title"] = "Editar turno";

            return view;
        }

        [Route("views/excluir/{id}")]
        public ViewResult Excluir(Guid id)
        {
            var view = View("~/Views/Turno/Excluir.cshtml");

            view.ViewData["title"] = "Excluir turno";
           
            return view;
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
                    return ObterErroIdObrigatorio();
                }

                var turno = await _turnoRepositorio.ObterPorId(id.Value);

                if (turno == null)
                {
                    var erroResponse = new ErrorResponse()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Mensagem = "Não encontrado"
                    };

                    erroResponse.AtribuirErro("idturno", "Não foi econtrado turno para esse ID");

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

        [HttpPost("inserir")]
        [ProducesResponseType(typeof(Turno), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Inserir([FromBody] Turno turnoParaInserir)
        {
            try
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
            catch (Exception ex)
            {
                var erroResponse = new ErrorResponse(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, erroResponse);
            }
        }

        [HttpPut("editar/{id}")]
        [ProducesResponseType(typeof(Turno), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Editar([FromRoute] Guid? id, [FromBody] Turno turno)
        {
            try
            {
                if (id == null)
                {
                    return ObterErroIdObrigatorio();
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
                    return ObterErroIdObrigatorio();
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
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        #region AUXILIAR

        private IActionResult ObterErroIdObrigatorio()
        {
            var errorResponse = new ErrorResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Mensagem = "Requisição inválida",
            };

            errorResponse.AtribuirErro("idturno", "id do turno é obrigatório");

            return BadRequest(errorResponse);
        }

        #endregion
    }
}
