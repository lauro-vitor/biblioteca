using Biblioteca.Dominio.ViewModel;
using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Servico;
using System.Net;

namespace Biblioteca.Servico
{
    public class TurnoServico : ITurnoServico
    {
        private readonly string _mensagemTurnoObrigatorio = "nome do turno é obrigatório";
        private readonly string _mensagemTurnoExiste = "nome do turno existe";

        public ErroViewModel? ValidarInserirTurno(Turno turnoParaInserir, IList<Turno> turnosCadastrados)
        {
            var erroResponse = new ErroViewModel();

            if (string.IsNullOrEmpty(turnoParaInserir.Nome))
            {
                erroResponse.AtribuirErro("nome", _mensagemTurnoObrigatorio);
            }

            if (turnosCadastrados.Any(t => t.Nome.ToLower().Trim().Equals(turnoParaInserir.Nome.ToLower().Trim())))
            {
                erroResponse.AtribuirErro("nome", _mensagemTurnoExiste);
            }

            if (erroResponse.Erros.Any())
            {
                erroResponse.Status = (int)HttpStatusCode.BadRequest;

                erroResponse.Mensagem = "Ocorreram alguns erros de validação";

                return erroResponse;
            }

            return null;
        }

        public ErroViewModel? ValidarEditarTurno(Turno turnoParaEditar, IList<Turno> turnosCadastrados)
        {
            var erroResponse = new ErroViewModel();

            if (turnoParaEditar.IdTurno == Guid.Empty)
            {
                erroResponse.AtribuirErro("idturno", "id do turno é obrigatório");
            }

            if (string.IsNullOrEmpty(turnoParaEditar.Nome))
            {
                erroResponse.AtribuirErro("nome", _mensagemTurnoObrigatorio);
            }

            if (turnosCadastrados.Any(t => t.IdTurno != turnoParaEditar.IdTurno
                                        && t.Nome.ToLower().Trim().Equals(turnoParaEditar.Nome.ToLower().Trim())))
            {
                erroResponse.AtribuirErro("nome", _mensagemTurnoExiste);
            }

            if (erroResponse.Erros.Any())
            {
                erroResponse.Status = (int)HttpStatusCode.BadRequest;

                erroResponse.Mensagem = "Ocorreram alguns erros de validação";

                return erroResponse;
            }

            return null;
        }

        public ErroViewModel? ValidarTurnoNaoExiste(Guid id, IList<Turno> turnoCadastrados)
        {
            var erroResponse = new ErroViewModel();

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
