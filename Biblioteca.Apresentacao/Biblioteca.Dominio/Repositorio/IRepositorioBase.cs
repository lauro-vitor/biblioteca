namespace Biblioteca.Dominio.Repositorio
{
	public interface IRepositorioBase <T> where T : class
	{
		Task Inserir(T obj);
		Task Editar(T obj);
		Task Excluir(Guid id);
		Task<T?> ObterPorId(Guid id);
	}
}
