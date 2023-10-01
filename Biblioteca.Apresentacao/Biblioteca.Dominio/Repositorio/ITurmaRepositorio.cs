using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;

namespace Biblioteca.Dominio.Repositorio
{
	public interface ITurmaRepositorio
	{
		public Pagination<Turma> Obter(
			string? idTurno = null,
			string? nome = null,
			int? periodo = null,
			string? sigla = null,
			string? ordenacaoCampo = null,
			string? ordenacaoDirecao = null,
			int? pageIndex = null,
			int? pageSize = null
		);

		public Task<Turma?> ObterPorId(Guid id);
		public Task Inserir(Turma turma);
		public Task Editar(Turma turma);
		public Task Excluir(Guid id);
	}
}
