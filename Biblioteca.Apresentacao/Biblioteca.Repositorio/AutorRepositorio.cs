using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.Autor;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Biblioteca.Repositorio
{
    public class AutorRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;

        public AutorRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<AutorViewModel> Editar(AutorViewModel autorViewModel)
        {
            await ValidarInserirEditar(autorViewModel);

            var autor = new Autor()
            {
                IdAutor = autorViewModel.IdAutor,
                Nome = autorViewModel.Nome
            };

            var autorParaEditar = await _context.Autor.FirstOrDefaultAsync(a => a.IdAutor == autor.IdAutor);

            if (autorParaEditar == null)
                throw new BibliotecaException("Autor não encontrado para edição");

            autorParaEditar.Nome = autor.Nome;

            await _context.SaveChangesAsync();

            
            return autorViewModel;
        }

        public async Task<AutorViewModel> Inserir(AutorViewModel autorViewModel)
        {
            await ValidarInserirEditar(autorViewModel);

            var autor = new Autor()
            {
                IdAutor = Guid.NewGuid(),
                Nome = autorViewModel.Nome
            };

            await _context.AddAsync(autor);

            await _context.SaveChangesAsync();

            var resultado = new AutorViewModel
            {
                IdAutor = autor.IdAutor,
                Nome = autor.Nome
            };

            return resultado;
        }

        public async Task Excluir(Guid id)
        {
            var autorParExcluir = await _context.Autor
                .Include(l => l.LivroAutores)
                .FirstOrDefaultAsync(a => a.IdAutor == id);

            if (autorParExcluir == null)
                throw new BibliotecaException("Autor não encontrado para exclusão");

            if (autorParExcluir.LivroAutores?.Any() ?? false)
                throw new BibliotecaException("O Autor possui livros vinculados a ele, portanto não é possível exclui-lo");

            _context.Autor.Remove(autorParExcluir);

            await _context.SaveChangesAsync();
        }

        public async Task<AutorViewModel?> ObterPorId(Guid id)
        {
            return await _context.Autor
                .AsNoTracking()
                .Select(a => new AutorViewModel
                {
                    IdAutor = a.IdAutor,
                    Nome = a.Nome
                })
                .FirstOrDefaultAsync(a => a.IdAutor == id);
        }

        public Pagination<AutorViewModel> Obter(AutorParametroViewModel parametro)
        {

            if (parametro == null)
                throw new BibliotecaException("parâmetro de autor inválido");

            var query = _context.Autor
                .AsNoTracking()
                .Select(a => new AutorViewModel
                {
                    IdAutor = a.IdAutor,
                    Nome = a.Nome
                });

            return ObterFiltroPaginacao(query, parametro);
        }

#nullable disable
        public static Pagination<AutorViewModel> ObterFiltroPaginacao(IQueryable<AutorViewModel> query, AutorParametroViewModel parametro)
        {

            if (!string.IsNullOrWhiteSpace(parametro.Nome))
            {
                string nome = parametro.Nome.ToLower().Trim();

                query = query.Where(a => a.Nome.ToLower().Trim().Contains(nome));
            }

            if (!string.IsNullOrWhiteSpace(parametro.SortProp))
            {
                Expression<Func<AutorViewModel, object>> sortFunc = null;

                switch (parametro.SortProp.Trim())
                {
                    case "nome":
                        sortFunc = l => l.Nome;
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

            var resultado = new Pagination<AutorViewModel>(query, parametro.PageIndex, parametro.PageSize);

            return resultado;
        }

        private async Task ValidarInserirEditar(AutorViewModel autorViewModel)
        {
            if (autorViewModel == null)
                throw new BibliotecaException("parêmetro de autorViewModel inválido");

            if (string.IsNullOrWhiteSpace(autorViewModel.Nome))
                return;

            var existeAutor = await _context.Autor
                .AsNoTracking()
                .AnyAsync(a => a.IdAutor != autorViewModel.IdAutor
                            && a.Nome.ToLower().Equals(autorViewModel.Nome.ToLower().Trim()));

            if (existeAutor)
                throw new BibliotecaException("Já existe um autor com este nome");
        }

        
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
