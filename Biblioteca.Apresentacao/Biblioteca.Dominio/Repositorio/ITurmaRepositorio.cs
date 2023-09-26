using Biblioteca.Dominio.Entidades;

namespace Biblioteca.Dominio.Repositorio
{
    public interface ITurmaRepositorio
    {
        public Task<IList<Turma>> Obter();
        public Task<Turma?> ObterPorId(Guid id);
        public Task Inserir(Turma turma);
        public Task Editar(Turma turma);
        public Task Excluir(Guid id);
    }
}
