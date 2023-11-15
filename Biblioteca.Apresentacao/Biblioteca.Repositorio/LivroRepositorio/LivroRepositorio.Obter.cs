using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        private IQueryable<LivroViewModel> ObterQueryLivroViewModel()
        {
#nullable disable
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

            if(!string.IsNullOrWhiteSpace(parametro.Genero))
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
            
                switch(parametro.SortProp.Trim())
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

                if(sortFunc != null)
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

    }
}
