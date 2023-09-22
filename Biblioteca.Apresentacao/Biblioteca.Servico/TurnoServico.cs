using Biblioteca.Dominio.DTO;
using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Servico;
using System.Net;

namespace Biblioteca.Servico
{
    public class TurnoServico : ITurnoServico
    {
        public ErrorResponse? ValidarInserirTurno(Turno turnoParaInserir, IList<Turno> turnosCadastrados)
        {
            var erroResponse = new ErrorResponse();

            if (string.IsNullOrEmpty(turnoParaInserir.Nome))
            {
                erroResponse.AtribuirErro("nome", "nome do turno é obrigatório");
            }

            if (turnosCadastrados.Any(t => t.Nome.ToLower().Trim().Equals(turnoParaInserir.Nome.ToLower().Trim())))
            {
                erroResponse.AtribuirErro("nome", "nome do turno existe");
            }

            if (erroResponse.Erros.Any())
            {
                erroResponse.Status = (int)HttpStatusCode.BadRequest;

                erroResponse.Mensagem = "Ocorreram alguns erros de validação";

                return erroResponse;
            }

            return null;
        }

        public ErrorResponse? ValidarEditarTurno(Turno turnoParaEditar, IList<Turno> turnosCadastrados)
        {
            var erroResponse = new ErrorResponse();

            if (turnoParaEditar.IdTurno == Guid.Empty)
            {
                erroResponse.AtribuirErro("idturno", "id do turno é obrigatório");
            }

            if (string.IsNullOrEmpty(turnoParaEditar.Nome))
            {
                erroResponse.AtribuirErro("nome", "nome do turno é obrigatório");
            }

            if (turnosCadastrados.Any(t => t.IdTurno != turnoParaEditar.IdTurno
                                        && t.Nome.ToLower().Trim().Equals(turnoParaEditar.Nome.ToLower().Trim())))
            {
                erroResponse.AtribuirErro("nome", "nome do turno existe");
            }

            if (erroResponse.Erros.Any())
            {
                erroResponse.Status = (int)HttpStatusCode.BadRequest;

                erroResponse.Mensagem = "Ocorreram alguns erros de validação";

                return erroResponse;
            }

            return null;
        }

        public ErrorResponse? ValidarTurnoNaoExiste(Guid id, IList<Turno> turnoCadastrados)
        {
            var erroResponse = new ErrorResponse();

            if (!turnoCadastrados.Any(t => t.IdTurno == id))
            {
                erroResponse.Status = (int)HttpStatusCode.NotFound;

                erroResponse.Mensagem = "Ocorreram alguns erros de validação";

                erroResponse.AtribuirErro("idturno", "Turno não encontrado para esse ID");

                return erroResponse;
            }

            return null;
        }
    }
}
