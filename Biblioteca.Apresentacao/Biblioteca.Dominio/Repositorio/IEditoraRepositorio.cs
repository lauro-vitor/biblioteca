using Biblioteca.Dominio.Entidades;

namespace Biblioteca.Dominio.Repositorio
{
	public interface IEditoraRepositorio
	{
		Task Inserir(Editora obj);
		Task Editar(Editora obj);
		Task Excluir(Guid id);
		Task<Editora?> ObterPorId(Guid id);
		Task<List<Editora>> ObterTodos();
	}
}
