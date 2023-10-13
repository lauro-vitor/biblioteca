using Biblioteca.Dominio.ViewModel;
using Biblioteca.Dominio.Entidades;

namespace Biblioteca.Dominio.Servico
{
    public interface ITurnoServico
    {
        public ErroViewModel? ValidarInserirTurno(Turno turnoParaInserir, IList<Turno> turnosCadastrados);

        public ErroViewModel? ValidarEditarTurno(Turno turnoParaEditar, IList<Turno> turnosCadastrados);

        public ErroViewModel? ValidarTurnoNaoExiste(Guid id, IList<Turno> turnoCadastrados);
    }
}
