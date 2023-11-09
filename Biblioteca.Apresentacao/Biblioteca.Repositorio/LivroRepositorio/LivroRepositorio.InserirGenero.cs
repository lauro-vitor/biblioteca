using Biblioteca.Dominio.Entidades;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        private async Task InserirGenero(Livro livro)
        {
            if (livro == null || livro.LivroGeneros == null || !livro.LivroGeneros.Any())
                return;

            foreach (var livroGenero in livro.LivroGeneros)
            {
                await _context.LivroGenero.AddAsync(livroGenero);
            }
        }
    }
}
