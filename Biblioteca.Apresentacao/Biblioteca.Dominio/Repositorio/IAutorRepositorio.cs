using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;

namespace Biblioteca.Dominio.Repositorio
{
	public interface IAutorRepositorio : IRepositorioBase<Autor>
	{
		public Pagination<Autor> ObterTodos(string? nome = null, int? pageIndex = null, int? pageSize = null);
	}
}
