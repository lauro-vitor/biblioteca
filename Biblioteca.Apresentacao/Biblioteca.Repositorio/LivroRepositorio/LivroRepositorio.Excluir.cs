using Biblioteca.Dominio.Objetos;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        public async Task Excluir(Guid id)
        {
            var livro = await _context.Livro.FirstOrDefaultAsync(l => l.IdLivro == id);

            if (livro == null)
                throw new BibliotecaException("Livro não encontrado para excluir");

            await ExcluirAutor(livro.IdLivro);
            await ExcluirGenero(livro.IdLivro);
            _context.Livro.Remove(livro);

            await _context.SaveChangesAsync();
        }
    }
}
