using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
	public class RepositorioBase<T> where T : class
	{
		private readonly BibliotecaContext _context;
		public RepositorioBase(BibliotecaContext context)
		{
			_context = context;
		}

		public async Task Inserir(T obj)
		{
			_context.Add(obj);

			await _context.SaveChangesAsync();
		}
		public async Task Editar(T obj)
		{
			_context.Update(obj);

			await _context.SaveChangesAsync();
		}

		public async Task Excluir(Guid id)
		{
			var objParaExcluir = await _context.FindAsync<T>(id);

			if (objParaExcluir == null)
			{
				throw new Exception("Objeto nao econtrado para exclusao");
			}

			_context.Remove(objParaExcluir);

			await _context.SaveChangesAsync();
		}

		public async Task<T?> ObterPorId(Guid id)
		{
			var resultado = await _context
				.Set<T>()
				.FindAsync(id);

			return resultado;
		}

		public async Task<List<T>>ObterTodos()
		{
			return await _context.Set<T>()
				.AsNoTracking()
				.ToListAsync();
		}


	}
}
