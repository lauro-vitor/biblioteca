using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Enums;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.AlunoContato;
using Biblioteca.Dominio.ViewModel.AlunoVM;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Biblioteca.Repositorio
{
    public class AlunoRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;

        public AlunoRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<AlunoUpSertViewModel> Inserir(AlunoUpSertViewModel alunoUpSertViewModel)
        {
            if (alunoUpSertViewModel == null)
                throw new BibliotecaException("Aluno inválido");

            Sexo sexo;

            Enum.TryParse(alunoUpSertViewModel.Sexo, out sexo);

            var aluno = new Aluno()
            {
                IdAluno = Guid.NewGuid(),
                Nome = alunoUpSertViewModel.Nome ?? string.Empty,
                Matricula = alunoUpSertViewModel.Matricula ?? string.Empty,
                DataNascimento = alunoUpSertViewModel.DataNascimento,
                Sexo = sexo
            };

            await _context.Aluno.AddAsync(aluno);

            await _context.SaveChangesAsync();

            alunoUpSertViewModel.IdAluno = aluno.IdAluno;

            return alunoUpSertViewModel;
        }

        public async Task<AlunoUpSertViewModel> Editar(AlunoUpSertViewModel alunoUpSertViewModel)
        {
            if (alunoUpSertViewModel == null)
                throw new BibliotecaException("Aluno inválido");

            var aluno = await _context.Aluno.FirstOrDefaultAsync(a => a.IdAluno == alunoUpSertViewModel.IdAluno);

            if (aluno == null)
                throw new BibliotecaException("Aluno não encontrado para editar");

            Sexo sexo;

            Enum.TryParse(alunoUpSertViewModel.Sexo, out sexo);

            aluno.Nome = alunoUpSertViewModel.Nome ?? string.Empty;
            aluno.Matricula = alunoUpSertViewModel.Matricula ?? string.Empty;
            aluno.DataNascimento = alunoUpSertViewModel.DataNascimento;
            aluno.Sexo = sexo;

            await _context.SaveChangesAsync();

            return alunoUpSertViewModel;
        }

        public async Task Excluir(Guid id)
        {
            if (id == Guid.Empty)
                throw new BibliotecaException("id do aluno inválido");

            var aluno = await _context.Aluno
                .Include(a => a.AlunoContatos)
                .FirstOrDefaultAsync(a => a.IdAluno == id);

            if (aluno == null)
                throw new BibliotecaException("O Aluno não foi encontrado para excluir");

            if (aluno.AlunoContatos != null && aluno.AlunoContatos.Any())
                throw new BibliotecaException("O Aluno possui contatos associado a seu cadastro, portanto não é possivel excluir");

            if (aluno.Emprestimos != null && aluno.Emprestimos.Any())
                throw new BibliotecaException("O aluno possui empréstimos associado a seu cadastro, portanto não é possível excluir");

            _context.Aluno.Remove(aluno);

            await _context.SaveChangesAsync();
        }

        public async Task Inativar(Guid id)
        {
            if (id == Guid.Empty)
                throw new BibliotecaException("id do aluno inválido");

            var aluno = await _context.Aluno.FirstOrDefaultAsync(a => a.IdAluno == id);

            if (aluno == null)
                throw new BibliotecaException("O Aluno não foi encontrado para inativar");

            aluno.Desativado = true;

            await _context.SaveChangesAsync();
        }

#nullable disable
        public async Task<AlunoQueryViewModel> ObterPorId(Guid id)
        {
            if (id == Guid.Empty)
                throw new BibliotecaException("id do aluno inválido");

            var aluno = await _context.Aluno
                .AsNoTracking()
                .Include(a => a.AlunoContatos)
                .ThenInclude(ac => ac.Parentesco)
                .Select(a => new AlunoQueryViewModel
                {
                    IdAluno = a.IdAluno,
                    DataNascimento = a.DataNascimento,
                    Matricula = a.Matricula,
                    Nome = a.Nome,
                    Sexo = a.Sexo,
                    Desativado = a.Desativado,
                    Contatos = a.AlunoContatos.Select(ac => new AlunoContatoViewModel
                    {
                        IdContato = ac.IdContato,
						IdAluno = ac.IdAluno,
						IdParentesco = ac.IdParentesco,
                        NomeParentesco = ac.Parentesco.Nome,
                        Nome = ac.Nome,
                        Email = ac.Email,
                        Telefone = ac.Telefone,
                        Observacao = ac.Observacao
                    })
                })
                .FirstOrDefaultAsync(a => a.IdAluno == id);

            return aluno;
        }

        public Pagination<AlunoQueryViewModel> Obter(AlunoParametroViewModel parametro)
        {
            var query = _context.Aluno
                .AsNoTracking()
                .Where(a => 
                    (string.IsNullOrWhiteSpace(parametro.Nome) || a.Nome.ToLower().Trim().Contains(parametro.Nome))
                    && 
                    (string.IsNullOrWhiteSpace(parametro.Matricula) || a.Matricula.ToLower().Trim().Contains(parametro.Matricula)
                    &&
                    (parametro.DataNascimentoInicio == null || a.DataNascimento >= parametro.DataNascimentoInicio)
                    && 
                    (parametro.DataNascimentoFim == null || a.DataNascimento <= parametro.DataNascimentoFim))
                    && 
                    (parametro.SomenteAtivos == null || (parametro.SomenteAtivos.Value && a.Desativado == false))
                );

            if (!string.IsNullOrWhiteSpace(parametro.SortProp))
            {
                Expression<Func<Aluno, object>> sortFunc = null;

                switch (parametro.SortProp)
                {
                    case "nome":
                        sortFunc = a => a.Nome;
                        break;
                    case "matricula":
                        sortFunc = a => a.Matricula;
                        break;
                    case "dataNascimento":
                        sortFunc = a => a.DataNascimento;
                        break;
                    case "desativado":
                        sortFunc = a => a.Desativado;
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


            var queryViewModel = query.Select(a => new AlunoQueryViewModel
            {
                IdAluno = a.IdAluno,
                Nome = a.Nome,
                Matricula = a.Matricula,
                DataNascimento = a.DataNascimento,
                Desativado = a.Desativado,
                Sexo = a.Sexo
            });

            var resultado = new Pagination<AlunoQueryViewModel>(queryViewModel, parametro.PageIndex, parametro.PageSize);

            return resultado;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
