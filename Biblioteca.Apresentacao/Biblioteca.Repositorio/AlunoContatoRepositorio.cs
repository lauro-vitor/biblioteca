using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.AlunoContato;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public class AlunoContatoRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;

        public AlunoContatoRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<AlunoContatoViewModel> Inserir(Guid idAluno, AlunoContatoViewModel alunoContatoViewModel)
        {
            if (alunoContatoViewModel == null)
                throw new BibliotecaException("Contato do aluno inválido");

            if (idAluno == Guid.Empty)
                throw new BibliotecaException("id do aluno inválido");

        
            var aluno = await _context.Aluno.FirstOrDefaultAsync(a => a.IdAluno == idAluno);

            if (aluno == null)
                throw new BibliotecaException("Aluno não encontrado");

            var parentesco = await _context.Parentesco.FirstOrDefaultAsync(p => p.IdParentesco == alunoContatoViewModel.IdParentesco);

            if (parentesco == null)
                throw new BibliotecaException("Parentesco não encontrado");

            var alunoContato = new AlunoContato()
            {
                IdContato = Guid.NewGuid(),
                IdAluno = idAluno,
                Aluno = aluno,
                IdParentesco = parentesco.IdParentesco,
                Parentesco = parentesco,
                Nome = alunoContatoViewModel.Nome ?? string.Empty,
                Telefone = alunoContatoViewModel.Telefone ?? string.Empty,
                Email = alunoContatoViewModel.Email,
                Observacao = alunoContatoViewModel.Observacao,
            };

            await _context.AlunoContato.AddAsync(alunoContato);

            await _context.SaveChangesAsync();

            alunoContatoViewModel.IdContato = alunoContato.IdContato;
            alunoContatoViewModel.IdAluno = alunoContato.IdAluno;

            return alunoContatoViewModel;
        }

        public async Task<AlunoContatoViewModel> Editar(Guid idAluno, AlunoContatoViewModel alunoContatoViewModel)
        {
            if (alunoContatoViewModel == null)
                throw new BibliotecaException("Contato do aluno inválido");

            if (idAluno == Guid.Empty)
                throw new BibliotecaException("id do aluno inválido");

            if (alunoContatoViewModel.IdContato == null || alunoContatoViewModel.IdContato == Guid.Empty)
                throw new BibliotecaException("id do contato do aluno inválido");

            var alunoContato = await _context.AlunoContato.FirstOrDefaultAsync(ac => ac.IdAluno == idAluno && ac.IdContato == alunoContatoViewModel.IdContato);

            if (alunoContato == null)
                throw new BibliotecaException("Contato do aluno não foi encontrado para edição");

            alunoContato.Nome = alunoContatoViewModel.Nome ?? string.Empty;
            alunoContato.Telefone = alunoContatoViewModel.Telefone ?? string.Empty;
            alunoContato.Email = alunoContatoViewModel.Email ?? string.Empty;
            alunoContato.Observacao = alunoContatoViewModel.Observacao ?? string.Empty;

            var parentesco = await _context.Parentesco.FirstOrDefaultAsync(p => p.IdParentesco == alunoContatoViewModel.IdParentesco);

            if (parentesco == null)
                throw new BibliotecaException("Parentesco não encontrado");

            alunoContato.IdParentesco = parentesco.IdParentesco;
            alunoContato.Parentesco = parentesco;

            await _context.SaveChangesAsync();

            alunoContatoViewModel.IdContato = alunoContato.IdContato;
            alunoContatoViewModel.IdAluno = alunoContato.IdAluno;

            return alunoContatoViewModel;
        }

        public async Task Excluir(Guid idAluno, Guid idContato)
        {
            if (idAluno == Guid.Empty)
                throw new BibliotecaException("id do aluno inválido");

            if (idContato == Guid.Empty)
                throw new BibliotecaException("id do contato do aluno inválido");

            var alunoContato = await _context.AlunoContato
                .FirstOrDefaultAsync(ac => ac.IdAluno == idAluno
                                        && ac.IdContato == idContato);
            if (alunoContato == null)
                throw new BibliotecaException("Erro ao excluir, não foi encontrado o contato para este aluno");

            _context.AlunoContato.Remove(alunoContato);

            await _context.SaveChangesAsync();
        }

#nullable disable

        public async Task<ICollection<AlunoContatoViewModel>> Obter(Guid idAluno, Guid? idContato)
        {
            var query = _context.AlunoContato
                .AsNoTracking()
                .Select(ac => new AlunoContatoViewModel
                {
                    IdAluno = ac.IdAluno,
                    IdContato = ac.IdContato,
                    Nome = ac.Nome,
                    Telefone = ac.Telefone,
                    Email = ac.Email,
                    Observacao = ac.Observacao,
                    IdParentesco = ac.IdParentesco,
                    NomeParentesco = ac.Parentesco.Nome
                })
                .Where(ac => ac.IdAluno == idAluno);
                
            if(idContato != null && idContato != Guid.Empty)
            {
                query = query.Where(ac => ac.IdContato == idContato);
            }

            return await query.ToListAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
