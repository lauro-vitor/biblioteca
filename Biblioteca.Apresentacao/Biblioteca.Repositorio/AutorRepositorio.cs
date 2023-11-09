using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public class AutorRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;

        public AutorRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<Autor> Editar(AutorViewModel autorViewModel)
        {
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

            return autorParaEditar;
        }

        public async Task Excluir(Guid id)
        {
            var autorParExcluir = await _context.Autor.FirstOrDefaultAsync(a => a.IdAutor == id);

            if (autorParExcluir == null)
                throw new BibliotecaException("Autor não encontrado para exclusão");

            _context.Autor.Remove(autorParExcluir);

            await _context.SaveChangesAsync();
        }

        public async Task<Autor> Inserir(AutorViewModel autorViewModel)
        {
            var autor = new Autor()
            {
                IdAutor = Guid.NewGuid(),
                Nome = autorViewModel.Nome
            };

            await _context.AddAsync(autor);

            await _context.SaveChangesAsync();

            return autor;
        }

        public async Task<Autor?> ObterPorId(Guid id)
        {
            return await _context.Autor
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.IdAutor == id);
        }

        public async Task<IEnumerable<Autor>> Obter()
        {
            return await _context.Autor
                .AsNoTracking()
                .ToListAsync();
        }

        public IEnumerable<Autor>? ObterAutores(IEnumerable<AutorViewModel>? autoresViewModel)
        {
            if (autoresViewModel == null || !autoresViewModel.Any())
                return null;

            var idsAutores = autoresViewModel
                .Select(a => a.IdAutor)
                .Distinct()
                .ToList();

            var autores = _context.Autor.Where(a => idsAutores.Contains(a.IdAutor));

            return autores;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
