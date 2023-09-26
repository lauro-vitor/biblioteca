using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Repositorio;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public class TurmaRepositorio : ITurmaRepositorio
    {
        private readonly BibliotecaContext _context;
        public TurmaRepositorio(BibliotecaContext context)
        {
            _context = context;
        }
        public async Task Editar(Turma turma)
        {
            var turmaParaEditar = await _context.Turma.FirstAsync(t => t.Id == turma.Id);

            turmaParaEditar.Nome = turma.Nome;
            turmaParaEditar.Periodo = turma.Periodo;
            turmaParaEditar.Sigla = turma.Sigla;

           await _context.SaveChangesAsync();
        }

        public async Task Excluir(Guid id)
        {
            var turmaParaExcluir = await _context.Turma.FirstAsync(t => t.Id == id);

            _context.Turma.Remove(turmaParaExcluir);

            await _context.SaveChangesAsync();
        }

        public async Task Inserir(Turma turma)
        {
            _context.Turma.Add(turma);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Turma>> Obter()
        {
            return await _context.Turma
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Turma?> ObterPorId(Guid id)
        {
            var turma = await _context.Turma
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            return turma;
        }
    }
}
