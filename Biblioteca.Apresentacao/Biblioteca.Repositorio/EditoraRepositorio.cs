using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Repositorio;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
	public class EditoraRepositorio : RepositorioBase<Editora>, IEditoraRepositorio
	{
		private readonly BibliotecaContext _context;
		public EditoraRepositorio(BibliotecaContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Editora>> ObterTodos()
		{
			return await _context.Editora
				.AsNoTracking()
				.ToListAsync();
		}

	}
}
