using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.Repositorio;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
			_context.Turma.Update(turma);

			await _context.SaveChangesAsync();
		}

		public async Task Excluir(Guid id)
		{
			var turmaParaExcluir = await _context.Turma.FirstAsync(t => t.IdTurma == id);

			_context.Turma.Remove(turmaParaExcluir);

			await _context.SaveChangesAsync();
		}

		public async Task Inserir(Turma turma)
		{
			_context.Turma.Add(turma);
			await _context.SaveChangesAsync();
		}

		public Pagination<Turma> Obter(
			string? idTurno = null,
			string? nome = null,
			int? periodo = null,
			string? sigla = null,
			string? ordenacaoCampo = null,
			string? ordenacaoDirecao = null,
			int? pageIndex = null,
			int? pageSize = null
		)
		{
			var query = _context.Turma
				.Include(t => t.Turno)
				.AsNoTracking();

			if (!string.IsNullOrEmpty(idTurno))
			{
				Guid idTurnoAux = Guid.Parse(idTurno);

				query = query.Where(t => t.IdTurno == idTurnoAux);
			}

			if (!string.IsNullOrEmpty(nome))
			{
				query = query.Where(t => t.Nome.ToLower().Trim().Contains(nome.ToLower().Trim()));
			}

			if (periodo.HasValue && periodo > 0)
			{
				query = query.Where(t => t.Periodo == periodo.Value);
			}

			if (!string.IsNullOrEmpty(sigla))
			{
				query = query.Where(t => t.Sigla.ToLower().Trim().Contains(sigla.ToLower().Trim()));
			}

			if (!string.IsNullOrEmpty(ordenacaoCampo))
			{
				Expression<Func<Turma, object>> funcOrder;

				switch (ordenacaoCampo)
				{
					case "nome":
						funcOrder = t => t.Nome;
						break;

					case "sigla":
						funcOrder = t => t.Sigla;
						break;

					case "periodo":
						funcOrder = t => t.Periodo;
						break;

					case "turnoNome":
						funcOrder = t => t.Turno.Nome;
						break;

					default:
						funcOrder = t => t.Nome;
						break;
				}

				if ("DESC".Equals(ordenacaoDirecao))
				{
					query = query.OrderByDescending(funcOrder);
				}
				else
				{
					query = query.OrderBy(funcOrder);
				}
			}

			var paginacao = new Pagination<Turma>(query, pageIndex, pageSize);

			return paginacao;
		}

		public async Task<Turma?> ObterPorId(Guid id)
		{
			var turma = await _context.Turma
				.Include(t => t.Turno)
				.AsNoTracking()
				.FirstOrDefaultAsync(t => t.IdTurma == id);

			return turma;
		}
	}
}
