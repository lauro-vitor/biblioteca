using Biblioteca.Dominio.Entidades;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        private void ExcluirAutor(Livro livro)
        {
            if (livro == null || livro.LivroAutores == null || !livro.LivroAutores.Any())
                return;

            foreach (var livroAutorItem in livro.LivroAutores)
            {
                _context.LivroAutor.Remove(livroAutorItem);
            }
        }
    }
}
