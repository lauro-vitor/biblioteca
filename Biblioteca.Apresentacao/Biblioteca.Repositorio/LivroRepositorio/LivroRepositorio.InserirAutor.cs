using Biblioteca.Dominio.Entidades;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        private async Task InserirAutor(Livro livro)
        {
            if (livro == null || livro.LivroAutores == null || !livro.LivroAutores.Any())
                return;

            foreach (var livroAutorItem in livro.LivroAutores)
            {
                await _context.LivroAutor.AddAsync(livroAutorItem);
            }
        }
    }
}
