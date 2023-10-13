namespace Biblioteca.Dominio.ViewModel
{
	public class TurmaViewModel
	{
		public Guid? IdTurma { get; set; }
		public Guid? IdTurno { get; set; }
		public string? Nome { get; set; }
		public int Periodo { get; set; }
		public string? Sigla { get; set; }
	}
}
