using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.ViewModel.Genero;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        private async Task<ICollection<LivroGenero>?> InserirGenero(Livro livro, ICollection<GeneroViewModel>? generosViewModel)
        {
            if (livro == null || generosViewModel == null || !generosViewModel.Any())
                return null;

            var generos = await _generoRepositorio.Obter(generosViewModel);

            if (generos == null)
                return null;

            var livroGeneros = new List<LivroGenero>();

            foreach (var genero in generos)
            {
                var livroGenero = new LivroGenero()
                {
                    IdLivro = livro.IdLivro,
                    IdGenero = genero.IdGenero,
                    Livro = livro,
                    Genero = genero
                };

                livroGeneros.Add(livroGenero);

                await _context.LivroGenero.AddAsync(livroGenero);
            }

            return livroGeneros;
        }
    }
}
