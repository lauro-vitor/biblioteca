using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.ViewModel.Autor;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        private async Task<ICollection<LivroAutor>?> InserirAutor(Livro livro, ICollection<AutorViewModel>? autoresViewModel)
        {
            if (livro == null || autoresViewModel == null || !autoresViewModel.Any())
                return null;

            var autores = await _autorRepositorio.Obter(autoresViewModel);

            if(autores == null) 
                return null;

            var livroAutores = new List<LivroAutor>();

            foreach (var autor in autores)
            {
                var livroAutor = new LivroAutor
                {
                    IdLivroAutor = Guid.NewGuid(),
                    IdLivro = livro.IdLivro,
                    IdAutor = autor.IdAutor,
                    Livro = livro,
                    Autor = autor,
                };

               livroAutores.Add(livroAutor);

               await _context.LivroAutor.AddAsync(livroAutor);
            }

            return livroAutores;
        }
    }
}
