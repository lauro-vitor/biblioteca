namespace Biblioteca.Dominio.ViewModel.AlunoVM
{
    public class AlunoViewModel
    {
        public Guid? IdAluno { get; set; }

        public string? Nome { get; set; }

        public string? Matricula { get; set; }

        public DateOnly? DataNascimento { get; set; }

        public string? Sexo { get; set; }

        public bool? Desativado { get; set; }
    }
}
