using Biblioteca.Dominio.DTO;
using Biblioteca.Dominio.Entidades;

namespace Biblioteca.Dominio.Servico
{
    public interface ITurnoServico
    {

        public Dictionary<string,List<string>>? ValidarInserir(Turno turno);

        public Dictionary<string,List<string>>? ValidarEditar(Turno turno);
    }
}
