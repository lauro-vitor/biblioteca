using Biblioteca.Dominio.ViewModel.AlunoVM;
using Biblioteca.Dominio.ViewModel.Livro;

namespace Biblioteca.Dominio.ViewModel.Emprestimo
{
    public class EmprestimoDetalheViewModel
    {
        public Guid IdEmprestimo { get; set; }

        public string? DataEmprestimo { get; set; }

        public string? DataDevolucao { get; set; }

        public LivroViewModel? Livro { get; set; }

        public AlunoQueryViewModel? Aluno { get; set; }
    }
}
