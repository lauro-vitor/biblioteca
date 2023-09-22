using Biblioteca.Dominio.DTO;
using Biblioteca.Dominio.Entidades;

namespace Biblioteca.Dominio.Servico
{
    public interface ITurnoServico
    {
        public ErrorResponse? ValidarInserirTurno(Turno turnoParaInserir, IList<Turno> turnosCadastrados);

        public ErrorResponse? ValidarEditarTurno(Turno turnoParaEditar, IList<Turno> turnosCadastrados);

        public ErrorResponse? ValidarTurnoNaoExiste(Guid id, IList<Turno> turnoCadastrados);
    }
}
