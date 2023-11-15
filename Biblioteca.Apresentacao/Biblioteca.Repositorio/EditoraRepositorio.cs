using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.Editora;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Biblioteca.Repositorio
{
    public class EditoraRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;

        public EditoraRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<EditoraViewModel> Inserir(EditoraViewModel editoraViewModel)
        {
            await ValidarInserirEditar(editoraViewModel);

            var editora = new Editora()
            {
                IdEditora = Guid.NewGuid(),
                Nome = editoraViewModel.Nome
            };

            await _context.Editora.AddAsync(editora);

            await _context.SaveChangesAsync();

            editoraViewModel.IdEditora = editora.IdEditora;

            return editoraViewModel;
        }

        public async Task<EditoraViewModel> Editar(EditoraViewModel editoraViewModel)
        {
            await ValidarInserirEditar(editoraViewModel);

            if (editoraViewModel.IdEditora == null)
                throw new BibliotecaException("idEditora: obrigatório");

            var editoraParaEditar = await _context.Editora.FirstOrDefaultAsync(e => e.IdEditora == editoraViewModel.IdEditora);

            if (editoraParaEditar == null)
                throw new BibliotecaException("Editora não encontrada para edição");

            editoraParaEditar.Nome = editoraViewModel.Nome;

            await _context.SaveChangesAsync();

            return editoraViewModel;
        }

        public async Task Excluir(Guid id)
        {
            var editoraParaExcluir = await _context.Editora
                .Include(e => e.Livros)
                .FirstOrDefaultAsync(e => e.IdEditora == id);

            if (editoraParaExcluir == null)
                throw new BibliotecaException("Editora não encontrada para exclusão");

            if (editoraParaExcluir.Livros != null && editoraParaExcluir.Livros.Any())
                throw new BibliotecaException("A editora possui livros vinculados a ela, portanto não é possível excluí-la");

            _context.Editora.Remove(editoraParaExcluir);

            await _context.SaveChangesAsync();
        }

      

        public async Task<EditoraViewModel?> ObterEditoraViewModelPorId(Guid id)
        {
            return await _context.Editora
                .AsNoTracking()
                .Select(e => new EditoraViewModel()
                {
                    IdEditora = e.IdEditora,
                    Nome = e.Nome
                })
                .FirstOrDefaultAsync(e => e.IdEditora == id);
        }

        public async Task<Editora> ObterEditoraPorId(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                throw new BibliotecaException("IdEditora: é obrigatório");

            var editora = await _context.Editora.FirstOrDefaultAsync(e => e.IdEditora == id);

            if (editora == null)
                throw new BibliotecaException("IdEditora: Editora não encontrada");

            return editora;
        }

        public Pagination<EditoraViewModel> Obter(EditoraParametroViewModel parametro)
        {
#nullable disable
            var query = _context.Editora
                  .AsNoTracking()
                  .Select(e => new EditoraViewModel
                  {
                      IdEditora = e.IdEditora,
                      Nome = e.Nome
                  });

            if (!string.IsNullOrWhiteSpace(parametro.Nome))
            {
                var nome = parametro.Nome.ToLower().Trim();

                query = query.Where(e => e.Nome.ToLower().Trim().Contains(nome));
            }

            if (!string.IsNullOrWhiteSpace(parametro.SortProp))
            {
                Expression<Func<EditoraViewModel, object>> sortFunc = null;

                switch (parametro.SortProp)
                {
                    case "nome":
                        sortFunc = e => e.Nome;
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

            var resultado = new Pagination<EditoraViewModel>(query, parametro.PageIndex, parametro.PageSize);

            return resultado;
        }

        private async Task ValidarInserirEditar(EditoraViewModel editoraViewModel)
        {
            if (editoraViewModel == null)
                throw new BibliotecaException("Parâmetro de editora não é válido");

            if (!string.IsNullOrWhiteSpace(editoraViewModel.Nome))
            {
                var nome = editoraViewModel.Nome.ToLower().Trim();

                var existeEditora = await _context.Editora.AnyAsync(e => e.IdEditora != editoraViewModel.IdEditora
                                                                && e.Nome.ToLower().Trim().Equals(nome));
                if (existeEditora)
                    throw new BibliotecaException("Já existe uma editora registrada com este nome");
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
