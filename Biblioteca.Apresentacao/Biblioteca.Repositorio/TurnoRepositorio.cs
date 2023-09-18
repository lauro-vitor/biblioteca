using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Repositorio;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public class TurnoRepositorio : ITurnoRepositorio
    {
        private readonly BibliotecaContext _context;
        public TurnoRepositorio(BibliotecaContext context)
        {
            _context = context;
        }
        public async Task Editar(Turno turno)
        {
            var turnoParaEditar = _context.Turno.FirstOrDefault(t => t.IdTurno == turno.IdTurno);

            if(turnoParaEditar == null)
            {
                throw new Exception("Não foi encontrado Turno para realizar a operação de edição");
            }

            turnoParaEditar.Nome = turno.Nome;
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(Guid id)
        {
            var turnoParaExcluir = _context.Turno.FirstOrDefault(t => t.IdTurno == id);

            if(turnoParaExcluir == null)
            {
                throw new Exception("Não foi encontrado Turno para realizar a operação de exclusão");
            }

            _context.Turno.Remove(turnoParaExcluir);
            await _context.SaveChangesAsync();
        }

        public async Task Inserir(Turno turno)
        {
            _context.Add(turno);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Turno>> Obter()
        {
            return await _context.Turno
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Turno?> ObterPorId(Guid id)
        {
            var turno = await _context.Turno
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.IdTurno == id);

   
            return turno;
        }
    }
}
