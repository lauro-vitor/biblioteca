using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Repositorio;
using Biblioteca.Repositorio.EntityFramework;

namespace Biblioteca.Repositorio
{
	public class EditoraRepositorio : RepositorioBase<Editora>, IEditoraRepositorio
	{
		public EditoraRepositorio(BibliotecaContext context) : base(context)
		{

		}
	}
}
