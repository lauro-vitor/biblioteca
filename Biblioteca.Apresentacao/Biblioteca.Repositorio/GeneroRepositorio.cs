using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public class GeneroRepositorio
    {
        private readonly BibliotecaContext _context;

        public GeneroRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genero>> Obter()
        {
            return await _context.Genero
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Genero?> ObterPorId(Guid id)
        {
            return await _context.Genero
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.IdGenero == id);
        }

        public async Task<Genero> Inserir(GeneroViewModel generoViewModel)
        {
            if (generoViewModel == null)
                throw new BibliotecaException("Genêro inválido");

            generoViewModel.IdGenero = Guid.NewGuid();

            var generoParaInserir = new Genero(generoViewModel);

            await _context.Genero.AddAsync(generoParaInserir);

            await _context.SaveChangesAsync();

            return generoParaInserir;
        }

        public async Task<Genero> Editar(GeneroViewModel generoViewModel)
        {
         
            var generoParaEditar = await _context.Genero.FirstAsync(g => g.IdGenero == generoViewModel.IdGenero);

            if (generoParaEditar == null)
                throw new BibliotecaException("Gênero não encontrado para edição");

            generoParaEditar.Nome = generoViewModel?.Nome ?? "";

            await _context.SaveChangesAsync();

            return generoParaEditar;
        }

        public async Task Excluir(Guid id)
        {
            var generoParaExcluir = await _context.Genero.FirstOrDefaultAsync(g => g.IdGenero == id);

            if (generoParaExcluir == null)
                throw new BibliotecaException("Gênero não encontrado para exclusão");

            _context.Genero.Remove(generoParaExcluir);

            await _context.SaveChangesAsync();
        }
    }
}
