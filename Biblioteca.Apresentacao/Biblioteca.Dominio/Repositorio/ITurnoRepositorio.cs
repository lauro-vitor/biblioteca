using Biblioteca.Dominio.Entidades;

namespace Biblioteca.Dominio.Repositorio
{
    public interface ITurnoRepositorio
    {
        public Task<IList<Turno>> Obter();
        public Task<Turno?> ObterPorId(Guid id);
        public Task Inserir(Turno turno);
        public Task Editar(Turno turno);
        public Task Excluir(Guid id);
    }
}
