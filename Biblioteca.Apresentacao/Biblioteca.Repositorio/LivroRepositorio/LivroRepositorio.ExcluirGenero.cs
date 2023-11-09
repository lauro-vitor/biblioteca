using Biblioteca.Dominio.Entidades;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        private void ExcluirGenero(Livro livro)
        {
            if (livro == null || livro.LivroGeneros == null || !livro.LivroGeneros.Any())
                return;

            foreach (var livroGenero in livro.LivroGeneros)
            {
                _context.LivroGenero.Remove(livroGenero);
            }
        }
    }
}
