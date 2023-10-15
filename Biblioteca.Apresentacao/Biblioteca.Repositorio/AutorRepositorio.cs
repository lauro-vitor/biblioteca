using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.Repositorio;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
	public class AutorRepositorio : RepositorioBase<Autor>, IAutorRepositorio
	{
		private readonly BibliotecaContext _context;
		public AutorRepositorio(BibliotecaContext context) : base(context)
		{
			_context = context;
		}

		public Pagination<Autor> ObterTodos(string? nome = null, int? pageIndex = null, int? pageSize = null)
		{
			var query = _context.Autor.AsNoTracking();

			if (!string.IsNullOrEmpty(nome))
			{
				query = query.Where(q => q.Nome != null && q.Nome.ToLower().Contains(nome.ToLower().Trim()));
			}

			query = query.OrderBy(q => q.Nome);

			var resultado = new Pagination<Autor>(query, pageIndex, pageSize);

			return resultado;
		}
	}
}
