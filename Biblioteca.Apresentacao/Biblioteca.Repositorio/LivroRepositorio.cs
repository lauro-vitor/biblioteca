using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.Autor;
using Biblioteca.Dominio.ViewModel.Editora;
using Biblioteca.Dominio.ViewModel.Genero;
using Biblioteca.Dominio.ViewModel.Livro;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;
        private readonly EditoraRepositorio _editoraRepositorio;

        public LivroRepositorio(BibliotecaContext context)
        {
            _context = context;
            _editoraRepositorio = new EditoraRepositorio(context);
        }

        private async Task ValidarInserirEditar(LivroInputViewModel livroViewModel)
        {
            if (livroViewModel.Editora == null)
                throw new BibliotecaException("Editora é obrigatória");

            var titulo = livroViewModel.Titulo?.ToLower()?.Trim() ?? string.Empty;

            var existeLivro = await _context.Livro.AnyAsync(l => l.IdLivro != livroViewModel.IdLivro
                                                              && l.Titulo.ToLower().Trim().Equals(titulo));

            if (existeLivro)
                throw new BibliotecaException("Já existe um livro com este título: " + titulo);

        }

        public async Task<LivroInputViewModel> Inserir(LivroInputViewModel livroInputViewModel)
        {
            if (livroInputViewModel == null)
                throw new BibliotecaException("Livro inválido");

            await ValidarInserirEditar(livroInputViewModel);

            var editora = await _editoraRepositorio.ObterEditoraPorId(livroInputViewModel?.Editora?.IdEditora);

            var livro = new Livro()
            {
                IdLivro = livroInputViewModel.IdLivro ?? Guid.NewGuid(),
                Titulo = livroInputViewModel.Titulo?.Trim() ?? string.Empty,
                DataPublicacao = livroInputViewModel.DataPublicacao ?? new DateOnly(),
                QuantidadeEstoque = livroInputViewModel.QuantidadeEstoque ?? -1,
                Edicao = livroInputViewModel.Edicao,
                Volume = livroInputViewModel.Volume,
                IdEditora = editora.IdEditora,
                Editora = editora
            };

            await _context.Livro.AddAsync(livro);

            await _context.SaveChangesAsync();

            livroInputViewModel.IdLivro = livro.IdLivro;

            return livroInputViewModel;
        }

        public async Task<LivroInputViewModel> Editar(LivroInputViewModel livroInputViewModel)
        {
            if (livroInputViewModel == null)
                throw new BibliotecaException("Livro inválido");

            if (livroInputViewModel.IdLivro == null || livroInputViewModel.IdLivro == Guid.Empty)
                throw new BibliotecaException("idLivro: inválido");

            await ValidarInserirEditar(livroInputViewModel);

            var editora = await _editoraRepositorio.ObterEditoraPorId(livroInputViewModel?.Editora?.IdEditora);

            var livro = await _context.Livro
              .Include(l => l.Editora)
              .FirstOrDefaultAsync(l => l.IdLivro == livroInputViewModel.IdLivro);

            if (livro == null)
                throw new BibliotecaException("idLivro: Não encontrado");

            livro.Titulo = livroInputViewModel.Titulo?.Trim() ?? string.Empty;
            livro.DataPublicacao = livroInputViewModel.DataPublicacao ?? new DateOnly();
            livro.QuantidadeEstoque = livroInputViewModel.QuantidadeEstoque ?? -1;
            livro.Edicao = livroInputViewModel.Edicao;
            livro.Volume = livroInputViewModel.Volume;
            livro.IdEditora = editora.IdEditora;
            livro.Editora = editora;

            await _context.SaveChangesAsync();


            return livroInputViewModel;
        }

        public async Task Excluir(Guid id)
        {
            var livro = await _context.Livro
                .Include(l => l.LivroAutores)
                .Include(l => l.LivroGeneros)
                .FirstOrDefaultAsync(l => l.IdLivro == id);

            if (livro == null)
                throw new BibliotecaException("idLivro: Livro não encontrado para excluir");

            if (livro.LivroAutores != null && livro.LivroAutores.Any())
                throw new BibliotecaException("O livro possui autores associados a ele, portanto não é possível excluir");

            if (livro.LivroGeneros != null && livro.LivroGeneros.Any())
                throw new BibliotecaException("O livro possui genêros associados a ele, portanto não é possível excluir");

            _context.Livro.Remove(livro);

            await _context.SaveChangesAsync();
        }

#nullable disable
        private IQueryable<LivroViewModel> ObterQueryLivroViewModel()
        {

            var query = _context.Livro
                    .AsNoTracking()
                    .Select(l => new LivroViewModel
                    {
                        IdLivro = l.IdLivro,
                        Titulo = l.Titulo,
                        DataPublicacao = l.DataPublicacao,
                        QuantidadeEstoque = l.QuantidadeEstoque,
                        Edicao = l.Edicao,
                        Volume = l.Volume,

                        Editora = new EditoraViewModel
                        {
                            IdEditora = l.IdEditora,
                            Nome = l.Editora.Nome ?? "",
                        },

                        Autores = l.LivroAutores.Select(la => new AutorViewModel
                        {
                            IdAutor = la.IdAutor,
                            Nome = la.Autor.Nome,
                        }).ToList(),

                        Generos = l.LivroGeneros.Select(lg => new GeneroViewModel()
                        {
                            IdGenero = lg.IdGenero,
                            Nome = lg.Genero.Nome,
                        }).ToList()
                    });

            return query;
        }


        public Pagination<LivroViewModel> Obter(LivroParametroViewModel parametro)
        {
            if (parametro == null)
                throw new BibliotecaException("parâmetro de livro inválido");

            var query = ObterQueryLivroViewModel();

            if (!string.IsNullOrWhiteSpace(parametro.Titulo))
            {
                var titulo = parametro.Titulo.ToLower().Trim();
                query = query.Where(l => l.Titulo.ToLower().Trim().Contains(titulo));
            }

            if (!string.IsNullOrWhiteSpace(parametro.Autor))
            {
                var autor = parametro.Autor.ToLower().Trim();
                query = query.Where(l => l.Autores.Any(a => a.Nome.ToLower().Trim().Contains(autor)));
            }

            if (!string.IsNullOrWhiteSpace(parametro.Genero))
            {
                var genero = parametro.Genero.ToLower().Trim();
                query = query.Where(l => l.Generos.Any(g => g.Nome.ToLower().Trim().Contains(genero)));
            }

            if (!string.IsNullOrWhiteSpace(parametro.Editora))
            {
                var editora = parametro.Editora.ToLower().Trim();
                query = query.Where(l => l.Editora.Nome.ToLower().Trim().Contains(editora));
            }

            if (!string.IsNullOrWhiteSpace(parametro.SortProp))
            {
                Expression<Func<LivroViewModel, object>> sortFunc = null;

                switch (parametro.SortProp.Trim())
                {
                    case "titulo":
                        sortFunc = l => l.Titulo;
                        break;
                    case "dataPublicacao":
                        sortFunc = l => l.DataPublicacao;
                        break;
                    case "quantidadeEstoque":
                        sortFunc = l => l.QuantidadeEstoque;
                        break;
                    case "edicao":
                        sortFunc = l => l.Edicao;
                        break;
                    case "volume":
                        sortFunc = l => l.Volume;
                        break;
                    case "editora":
                        sortFunc = l => l.Editora.Nome;
                        break;
                }

                if (sortFunc != null)
                {
                    if ("desc".Equals(parametro.SortDirection?.ToLower().Trim()))
                        query = query.OrderByDescending(sortFunc);
                    else
                        query = query.OrderBy(sortFunc);
                }
            }

            var pagincao = new Pagination<LivroViewModel>(query, parametro.PageIndex, parametro.PageSize);

            return pagincao;
        }

        public async Task<LivroViewModel> ObterPorId(Guid id)
        {
            var query = ObterQueryLivroViewModel();

            return await query.FirstOrDefaultAsync(l => l.IdLivro == id);
        }
        public void Dispose()
        {
            _context?.Dispose();
            _editoraRepositorio?.Dispose();
        }
    }
}
