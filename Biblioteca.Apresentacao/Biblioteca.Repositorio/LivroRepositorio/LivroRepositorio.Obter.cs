using Biblioteca.Dominio.ViewModel;
using Microsoft.EntityFrameworkCore;

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


        public async Task<IEnumerable<LivroViewModel>> Obter()
        {
            var query = ObterQueryLivroViewModel();

            return await query.ToListAsync();
        }

      

    }
}
