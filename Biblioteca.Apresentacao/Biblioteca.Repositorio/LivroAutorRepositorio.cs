using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.Autor;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public class LivroAutorRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;

        public LivroAutorRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Insere autores vinculados ao livro
        /// </summary>
        /// <param name="idLivro"></param>
        /// <param name="autoresViewModel"></param>
        /// <returns></returns>
        /// <exception cref="BibliotecaException"></exception>
        public async Task Inserir(Guid idLivro, ICollection<AutorViewModel>? autoresViewModel)
        {
            if (idLivro == Guid.Empty)
                throw new BibliotecaException("idLivro: inválido");

            if (autoresViewModel == null || !autoresViewModel.Any())
                throw new BibliotecaException("autoresViewModel inválido");

            var idsAutores = autoresViewModel
                .Select(a => a.IdAutor)
                .Distinct();

            var autores = await _context.Autor.Where(a => idsAutores.Contains(a.IdAutor)).ToListAsync();

            var livro = await _context.Livro.FirstOrDefaultAsync(l => l.IdLivro == idLivro);

            if (livro == null || autores == null || !autores.Any())
                return;

            bool existeAutorInserido = await _context.LivroAutor
                .AsNoTracking()
                .AnyAsync(la => la.IdLivro == idLivro && idsAutores.Contains(la.IdAutor));

            if (existeAutorInserido)
                throw new BibliotecaException("Um dos autores já foi associado a este livro");


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

                await _context.LivroAutor.AddAsync(livroAutor);
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Exclui os autores vinculados ao livro
        /// </summary>
        /// <param name="idLivro"></param>
        /// <param name="autoresViewModel"></param>
        /// <returns></returns>
        /// <exception cref="BibliotecaException"></exception>
        public async Task Excluir(Guid idLivro, ICollection<AutorViewModel>? autoresViewModel)
        {
            if (idLivro == Guid.Empty)
                throw new BibliotecaException("idLivro: inválido");

            if (autoresViewModel == null || !autoresViewModel.Any())
                throw new BibliotecaException("autoresViewModel inválido");

            var idsAutores = autoresViewModel
             .Select(a => a.IdAutor)
             .Distinct();

            if (idsAutores == null || !idsAutores.Any())
                return;

            var livroAutores = await _context.LivroAutor
                .Where(l => l.IdLivro == idLivro && idsAutores.Contains(l.IdAutor))
                .ToListAsync();

            if (livroAutores == null || livroAutores.Count <= 0)
                throw new BibliotecaException("Não existem autores para serem desvinculados do livro");

            foreach (var livroAutorItem in livroAutores)
            {
                _context.LivroAutor.Remove(livroAutorItem);
            }

            await _context.SaveChangesAsync();
        }

#nullable disable
        /// <summary>
        /// Obtem autores que não estão vinculados ao Livro
        /// </summary>
        /// <returns></returns>
        public Pagination<AutorViewModel> ObterAutoresDisponiveis(Guid idLivro, AutorParametroViewModel parametro)
        {
            var query = _context.Autor
                .Include(a => a.LivroAutores)
                .AsNoTracking()
                .Where(a => !a.LivroAutores.Any(la => la.IdLivro == idLivro))
                .Select(a => new AutorViewModel()
                {
                    IdAutor = a.IdAutor,
                    Nome = a.Nome
                });

            return AutorRepositorio.ObterFiltroPaginacao(query, parametro);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
