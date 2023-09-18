using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Servico;

namespace Biblioteca.Servico
{
    public class TurnoServico : ITurnoServico
    {
        public Dictionary<string, List<string>>? ValidarInserir(Turno turno)
        {
            var erros = new Dictionary<string, List<string>>();

            if (string.IsNullOrEmpty(turno.Nome))
            {
                erros.Add("nome", new List<string>() { "Nome do turno é obrigatório" });
            }

            if (erros.Any())
            {
                return erros;
            }

            return null;
        }

        public Dictionary<string, List<string>>? ValidarEditar(Turno turno)
        {
            var erros = new Dictionary<string, List<string>>();

            if (turno.IdTurno == null)
            {
                erros.Add("idTurno", new List<string> { "Id do turno é obrigatório" });
            }

            if (string.IsNullOrEmpty(turno.Nome))
            {
                erros.Add("nome", new List<string>() { "Nome do turno é obrigatório" });
            }

            if (erros.Any())
            {
                return erros;   
            }

            return null;
        }
    }
}
