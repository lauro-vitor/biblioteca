using Biblioteca.Dominio.ViewModel.AlunoContato;

namespace Biblioteca.Dominio.ViewModel.AlunoVM
{
    public class AlunoQueryViewModel
    {
        public Guid? IdAluno { get; set; }

        public string? Nome { get; set; }

        public string? Matricula { get; set; }

        public DateTime? DataNascimento { get; set; }

        public Enum? Sexo { get; set; }

        public bool? Desativado { get; set; }

        public IEnumerable<AlunoContatoViewModel>? Contatos { get; set; } 
    }
}
