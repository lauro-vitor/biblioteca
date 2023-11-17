using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.Parentesco;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Biblioteca.Repositorio
{
    public class ParentescoRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;

        public ParentescoRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<ParentescoViewModel> Inserir(ParentescoViewModel parentescoViewModel)
        {
            if (parentescoViewModel == null)
                throw new BibliotecaException("Parentesco inválido");

            await ValidarInserirEditar(parentescoViewModel);

            var parentesco = new Parentesco()
            {
                IdParentesco = Guid.NewGuid(),
                Nome = parentescoViewModel.Nome ?? string.Empty
            };

            await _context.Parentesco.AddAsync(parentesco);
            await _context.SaveChangesAsync();

            parentescoViewModel.IdParentesco = parentesco.IdParentesco;

            return parentescoViewModel;
        }

        public async Task<ParentescoViewModel> Editar(ParentescoViewModel parentescoViewModel)
        {
            if (parentescoViewModel == null)
                throw new BibliotecaException("Parentesco inválido");

            var parentesco = await _context.Parentesco
                .FirstOrDefaultAsync(p => p.IdParentesco == parentescoViewModel.IdParentesco);

            if (parentesco == null)
                throw new BibliotecaException("Parentesco não encontrado para editar");

            await ValidarInserirEditar(parentescoViewModel);

            parentesco.Nome = parentescoViewModel.Nome ?? "";

            await _context.SaveChangesAsync();

            return parentescoViewModel;
        }

        public async Task Excluir(Guid id)
        {
            var parentesco = await _context.Parentesco
                .Include(p => p.AlunoContatos)
                .FirstOrDefaultAsync(p => p.IdParentesco == id);

            if (parentesco == null)
                throw new BibliotecaException("Parentesco não encontrado para excluir");

            if (parentesco.AlunoContatos != null && parentesco.AlunoContatos.Any())
                throw new BibliotecaException("Existem contatos associado a este parentesco, portanto não será possível excluir");

            _context.Parentesco.Remove(parentesco);

            await _context.SaveChangesAsync();
        }

        public async Task<ParentescoViewModel?> ObterPorId(Guid id)
        {
            var result = await _context.Parentesco
                .AsNoTracking()
                .Select(p => new ParentescoViewModel
                {
                    IdParentesco = p.IdParentesco,
                    Nome = p.Nome
                })
                .FirstOrDefaultAsync(p => p.IdParentesco == id);

            return result;
        }


#nullable disable
        public Pagination<ParentescoViewModel> Obter(ParentescoParametroViewModel parametro)
        {
            var query = _context.Parentesco
                .AsNoTracking()
                .Select(p => new ParentescoViewModel
                {
                    IdParentesco = p.IdParentesco,
                    Nome = p.Nome
                });

            if (!string.IsNullOrWhiteSpace(parametro?.Nome))
            {
                var nome = parametro.Nome.ToLower().Trim();

                query = query.Where(p => p.Nome.ToLower().Trim().Contains(nome));
            }

            if (!string.IsNullOrWhiteSpace(parametro?.SortProp))
            {
                Expression<Func<ParentescoViewModel, object>> funcSort = null;

                if (parametro.SortProp.ToLower().Trim() == "nome")
                    funcSort = p => p.Nome;

                if (funcSort != null)
                {
                    if ("desc" == parametro.SortDirection?.ToLower()?.Trim())
                        query = query.OrderByDescending(funcSort);
                    else
                        query = query.OrderBy(funcSort);
                }
            }

            var paginacao = new Pagination<ParentescoViewModel>(query, parametro?.PageIndex, parametro?.PageSize);

            return paginacao;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        private async Task ValidarInserirEditar(ParentescoViewModel parentescoViewModel)
        {
            if (!string.IsNullOrWhiteSpace(parentescoViewModel.Nome))
            {
                var existeParentesco = await _context.Parentesco
                    .AsNoTracking()
                    .AnyAsync(p => p.IdParentesco != parentescoViewModel.IdParentesco
                                && p.Nome.ToLower() == parentescoViewModel.Nome.ToLower());

                if (existeParentesco)
                    throw new BibliotecaException("Já existe um parentesco com este nome");
            }
        }
    }
}
