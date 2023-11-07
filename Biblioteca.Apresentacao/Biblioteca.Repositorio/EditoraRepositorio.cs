using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public class EditoraRepositorio
    {
        private readonly BibliotecaContext _context;

        public EditoraRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<Editora> Editar(EditoraViewModel editoraViewModel)
        {
            var editora = new Editora()
            {
                IdEditora = editoraViewModel.IdEditora,
                Nome = editoraViewModel.Nome
            };

            var editoraParaEditar = await _context.Editora.FirstOrDefaultAsync(e => e.IdEditora == editora.IdEditora);

            if (editoraParaEditar == null)
                throw new BibliotecaException("Editora não encontrada para edição");

            editoraParaEditar.Nome = editora.Nome;

            await _context.SaveChangesAsync();

            return editora;
        }

        public async Task Excluir(Guid id)
        {
            var editoraParaExcluir = await _context.Editora.FirstOrDefaultAsync(e => e.IdEditora == id);

            if (editoraParaExcluir == null)
                throw new BibliotecaException("Editora não encontrada para exclusão");

            _context.Editora.Remove(editoraParaExcluir);

            await _context.SaveChangesAsync();
        }

        public async Task<Editora> Inserir(EditoraViewModel editoraViewModel)
        {
            var editora = new Editora()
            {
                IdEditora = Guid.NewGuid(),
                Nome = editoraViewModel.Nome
            };

            await _context.Editora.AddAsync(editora);

            await _context.SaveChangesAsync();

            return editora;
        }

        public async Task<IEnumerable<EditoraViewModel>> Obter()
        {
            return await _context.Editora
                .AsNoTracking()
                .Select(e => new EditoraViewModel
                {
                    IdEditora = e.IdEditora,
                    Nome = e.Nome
                })
                .ToListAsync();
        }

        public async Task<EditoraViewModel?> ObterPorId(Guid id)
        {
            return await _context.Editora
                .AsNoTracking()
                .Select(e => new EditoraViewModel() 
                { 
                    IdEditora = e.IdEditora,
                    Nome =  e.Nome
                })
                .FirstOrDefaultAsync(e => e.IdEditora == id);
        }
    }
}
