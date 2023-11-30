namespace Biblioteca.Dominio.ViewModel.AlunoVM
{
    public class AlunoUpSertViewModel
    {
        public Guid? IdAluno { get; set; }

        public string? Nome { get; set; }

        public string? Matricula { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string? Sexo { get; set; }

        public bool? Desativado { get; set; }
    }
}
