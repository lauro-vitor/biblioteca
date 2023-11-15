using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        private async Task ExcluirAutor(Guid? idLivro)
        {
            var livroAutores = await _context.LivroAutor.Where(l => l.IdLivro == idLivro).ToListAsync();

            if (livroAutores == null || !livroAutores.Any())
                return;

            foreach (var livroAutorItem in livroAutores)
            {
                _context.LivroAutor.Remove(livroAutorItem);
            }
        }
    }
}
