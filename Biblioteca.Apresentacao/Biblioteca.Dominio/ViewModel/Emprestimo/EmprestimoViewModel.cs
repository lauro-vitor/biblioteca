namespace Biblioteca.Dominio.ViewModel.Emprestimo
{
    public class EmprestimoViewModel
    {
        public Guid IdEmprestimo { get; set; }

        public string? DataEmprestimo { get; set; }

        public string? DataDevolucao { get; set; }

        public string? LivroTitulo { get; set; }

        public string? LivroEditora { get; set; }

        public string? AlunoNome { get; set; }

        public string? AlunoMatricula { get; set; }
    }
}
