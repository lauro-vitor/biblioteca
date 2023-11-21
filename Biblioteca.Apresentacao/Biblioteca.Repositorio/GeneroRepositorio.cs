using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.Genero;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Biblioteca.Repositorio
{
    public class GeneroRepositorio : IDisposable
    {

        private readonly BibliotecaContext _context;

        public GeneroRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<GeneroViewModel> Inserir(GeneroViewModel generoViewModel)
        {
            await ValidarInserirEditar(generoViewModel);

            generoViewModel.IdGenero = Guid.NewGuid();

            var generoParaInserir = new Genero(generoViewModel);

            await _context.Genero.AddAsync(generoParaInserir);

            await _context.SaveChangesAsync();

            return generoViewModel;
        }

        public async Task<GeneroViewModel?> Editar(GeneroViewModel generoViewModel)
        {
            await ValidarInserirEditar(generoViewModel);

            if (generoViewModel.IdGenero == null)
                throw new BibliotecaException("idGenero: Obrigatório");

            var generoParaEditar = await _context.Genero.FirstAsync(g => g.IdGenero == generoViewModel.IdGenero);

            if (generoParaEditar == null)
                throw new BibliotecaException("Gênero não encontrado para edição");

            generoParaEditar.Nome = generoViewModel?.Nome ?? "";

            await _context.SaveChangesAsync();

            return generoViewModel;
        }

        public async Task Excluir(Guid id)
        {
            var generoParaExcluir = await _context.Genero
                .Include(g => g.LivroGeneros)
                .FirstOrDefaultAsync(g => g.IdGenero == id);

            if (generoParaExcluir == null)
                throw new BibliotecaException("Gênero não encontrado para exclusão");

            if (generoParaExcluir.LivroGeneros != null && generoParaExcluir.LivroGeneros.Any())
                throw new BibliotecaException("O genêro possui livros vinculados a ele, portanto não é possível exclui-lo");

            _context.Genero.Remove(generoParaExcluir);

            await _context.SaveChangesAsync();
        }

        private async Task ValidarInserirEditar(GeneroViewModel generoViewModel)
        {
            if (generoViewModel == null)
                throw new BibliotecaException("Genêro inválido");

            if (!string.IsNullOrWhiteSpace(generoViewModel.Nome))
            {
                var generoNome = generoViewModel.Nome?.ToLower().Trim();

                var existeGenero = await _context.Genero.AnyAsync(g => g.IdGenero != generoViewModel.IdGenero
                                                                    && g.Nome.ToLower().Trim().Equals(generoNome));

                if (existeGenero)
                    throw new BibliotecaException("Já existe um genêro com este nome");
            }
        }

        public async Task<GeneroViewModel?> ObterPorId(Guid id)
        {
            return await _context.Genero
                .AsNoTracking()
                .Select(g => new GeneroViewModel
                {
                    IdGenero = g.IdGenero,
                    Nome = g.Nome
                })
                .FirstOrDefaultAsync(g => g.IdGenero == id);
        }

#nullable disable
        public Pagination<GeneroViewModel> Obter(GeneroParametroViewModel parametro)
        {
            var query = _context.Genero
                .Select(g => new GeneroViewModel
                {
                    IdGenero = g.IdGenero,
                    Nome = g.Nome
                })
                .AsNoTracking();

            return ObterFiltroPaginacao(query, parametro);
        }

        public static Pagination<GeneroViewModel> ObterFiltroPaginacao(IQueryable<GeneroViewModel> query, GeneroParametroViewModel parametro)
        {
            if (!string.IsNullOrWhiteSpace(parametro.Nome))
            {
                var nome = parametro.Nome.ToLower().Trim();

                query = query.Where(g => g.Nome.ToLower().Trim().Contains(nome));
            }

            if (!string.IsNullOrWhiteSpace(parametro.SortProp))
            {
                Expression<Func<GeneroViewModel, object>> sortFunc = null;

                switch (parametro.SortProp.Trim())
                {
                    case "nome":
                        sortFunc = g => g.Nome;
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

            var resultado = new Pagination<GeneroViewModel>(query, parametro.PageIndex, parametro.PageSize);

            return resultado;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }


}
