using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Enums;
using Biblioteca.Dominio.Objetos;
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

        public async Task<AlunoViewModel> Inserir(AlunoViewModel alunoViewModel)
        {
            if (alunoViewModel == null)
                throw new BibliotecaException("Aluno inválido");

            Sexo sexo;

            Enum.TryParse(alunoViewModel.Sexo, out sexo);

            var aluno = new Aluno()
            {
                IdAluno = Guid.NewGuid(),
                Nome = alunoViewModel.Nome ?? string.Empty,
                Matricula = alunoViewModel.Matricula ?? string.Empty,
                DataNascimento = alunoViewModel.DataNascimento,
                Sexo = sexo
            };

            await _context.Aluno.AddAsync(aluno);

            await _context.SaveChangesAsync();

            alunoViewModel.IdAluno = aluno.IdAluno;

            return alunoViewModel;
        }

        public async Task<AlunoViewModel> Editar(AlunoViewModel alunoViewModel)
        {
            if (alunoViewModel == null)
                throw new BibliotecaException("Aluno inválido");

            var aluno = await _context.Aluno.FirstOrDefaultAsync(a => a.IdAluno == alunoViewModel.IdAluno);

            if (aluno == null)
                throw new BibliotecaException("Aluno não encontrado para editar");

            Sexo sexo;

            Enum.TryParse(alunoViewModel.Sexo, out sexo);

            aluno.Nome = alunoViewModel.Nome ?? string.Empty;
            aluno.Matricula = alunoViewModel.Matricula ?? string.Empty;
            aluno.DataNascimento = alunoViewModel.DataNascimento;
            aluno.Sexo = sexo;

            await _context.SaveChangesAsync();

            return alunoViewModel;
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
        public async Task<AlunoViewModel> ObterPorId(Guid id)
        {
            if (id == Guid.Empty)
                throw new BibliotecaException("id do aluno inválido");

            var aluno = await _context.Aluno
                .AsNoTracking()
                .Include(a => a.AlunoContatos)
                .ThenInclude(ac => ac.Parentesco)
                .Select(a => new AlunoViewModel
                {
                    IdAluno = a.IdAluno,
                    DataNascimento = a.DataNascimento,
                    Matricula = a.Matricula,
                    Nome = a.Nome,
                    Sexo = a.Sexo.ToString(),
                    Desativado = a.Desativado
                })
                .FirstOrDefaultAsync(a => a.IdAluno == id);

            return aluno;
        }

        public Pagination<AlunoViewModel> Obter(AlunoParametroViewModel parametro)
        {
            var query = _context.Aluno
                .Select(a => new AlunoViewModel
                {
                    IdAluno = a.IdAluno,
                    Nome = a.Nome,
                    Matricula = a.Matricula,
                    DataNascimento = a.DataNascimento,
                    Desativado = a.Desativado,
                    Sexo = a.Sexo.ToString()
                })
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(parametro.Nome))
            {
                var nome = parametro.Nome.ToLower().Trim();

                query = query.Where(a => a.Nome.ToLower().Trim().Contains(nome));
            }

            if (!string.IsNullOrWhiteSpace(parametro.Matricula))
            {
                var matricula = parametro.Matricula.ToLower().Trim();

                query = query.Where(a => a.Matricula.ToLower().Trim().Contains(matricula));
            }

            if (!string.IsNullOrWhiteSpace(parametro.DataNascimentoInicio))
            {
                try
                {
                    var dataNascimentoInicio = DateTime.Parse(parametro.DataNascimentoInicio);

                    query = query.Where(a => a.DataNascimento >= dataNascimentoInicio);
                }
                catch
                {
                    throw new BibliotecaException("dataNascimentoInicio: parâmetro inválido");
                }
            }

            if (!string.IsNullOrWhiteSpace(parametro.DataNascimentoFim))
            {
                try
                {
                    var dataNascimentoFim = DateTime.Parse(parametro.DataNascimentoFim);

                    query = query.Where(a => a.DataNascimento <= dataNascimentoFim);
                }
                catch
                {
                    throw new BibliotecaException("dataNascimentoFim: parametro inválido");
                }
            }

            if (parametro.SomenteAtivos ?? false)
            {
                query = query.Where(a => a.Desativado == false);
            }

            if (!string.IsNullOrWhiteSpace(parametro.SortProp))
            {
                Expression<Func<AlunoViewModel, object>> sortFunc;

                var sortDic = new Dictionary<string, Expression<Func<AlunoViewModel, object>>>
                {
                    { "nome",  a => a.Nome },
                    { "matricula", a => a.Matricula},
                    { "dataNascimento", a => a.DataNascimento},
                    { "desativado", a => a.Desativado }
                };

                try
                {
					sortFunc = sortDic[parametro.SortProp];
				}
                catch
                {
                    sortFunc = null;
				}

                if (sortFunc != null)
                {
                    if ("desc".Equals(parametro.SortDirection?.ToLower().Trim()))
                        query = query.OrderByDescending(sortFunc);
                    else
                        query = query.OrderBy(sortFunc);
                }
            }

            var resultado = new Pagination<AlunoViewModel>(query, parametro.PageIndex, parametro.PageSize);

            return resultado;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
