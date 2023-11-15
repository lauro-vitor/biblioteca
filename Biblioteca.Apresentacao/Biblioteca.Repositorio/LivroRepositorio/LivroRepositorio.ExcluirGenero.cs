using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        private async Task ExcluirGenero(Guid? idLivro)
        {
            var livroGeneros = await _context.LivroGenero.Where(l => l.IdLivro == idLivro).ToListAsync();

            if (livroGeneros == null || !livroGeneros.Any())
                return;

            foreach (var livroGenero in livroGeneros)
            {
                _context.LivroGenero.Remove(livroGenero); 
            }
        }
    }
}
