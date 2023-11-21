using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.Genero;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public class LivroGeneroRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;

        public LivroGeneroRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task Inserir(Guid idLivro, ICollection<GeneroViewModel> generosViewModel)
        {
            if (idLivro == Guid.Empty)
                throw new BibliotecaException("idLivro: inválido");

            if (generosViewModel == null || !generosViewModel.Any())
                throw new BibliotecaException("generosViewModel: inválido");

            var idsGeneros = generosViewModel
                .Select(a => a.IdGenero)
                .Distinct();

            var generos = await _context.Genero.Where(g => idsGeneros.Contains(g.IdGenero)).ToListAsync();

            var livro = await _context.Livro.FirstOrDefaultAsync(l => l.IdLivro == idLivro);

            if (livro == null || generos == null || !generos.Any())
                return;

            bool existeGeneroInserido = await _context.LivroGenero
                .AsNoTracking()
                .AnyAsync(lg => lg.IdLivro == idLivro && idsGeneros.Contains(lg.IdGenero));

            if (existeGeneroInserido)
                throw new BibliotecaException("Um dos genêros já foi associado a este livro");

            foreach (var genero in generos)
            {
                var livroGenero = new LivroGenero
                {
                    IdLivroGenero = Guid.NewGuid(),
                    IdLivro = livro.IdLivro,
                    Livro = livro,
                    IdGenero = genero.IdGenero,
                    Genero = genero,
                };

                await _context.LivroGenero.AddAsync(livroGenero);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Excluir(Guid idLivro, ICollection<GeneroViewModel> generosViewModel)
        {
            if (idLivro == Guid.Empty)
                throw new BibliotecaException("idLivro: inválido");

            if (generosViewModel == null || !generosViewModel.Any())
                throw new BibliotecaException("generosViewModel: inválido");

            var idsGeneros = generosViewModel
                .Select(a => a.IdGenero)
                .Distinct();

            var livrosGeneros = _context.LivroGenero.Where(lg => lg.IdLivro == idLivro && idsGeneros.Contains(lg.IdGenero));

            if (livrosGeneros == null || !livrosGeneros.Any())
                throw new BibliotecaException("Nehum gênero foi encontrado para ser excluído");

            foreach (var livroGenero in livrosGeneros)
            {
                _context.LivroGenero.Remove(livroGenero);
            }

            await _context.SaveChangesAsync();
        }

#nullable disable
        public Pagination<GeneroViewModel> ObterGenerosDisponiveis(Guid idLivro, GeneroParametroViewModel parametro)
        {
            var query = _context.Genero
                .Include(g => g.LivroGeneros)
                .AsNoTracking()
                .Where(g => !g.LivroGeneros.Any(lg => lg.IdLivro == idLivro))
                .Select(g => new GeneroViewModel 
                { 
                    IdGenero = g.IdGenero,
                    Nome = g.Nome
                });

            return GeneroRepositorio.ObterFiltroPaginacao(query, parametro);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
