
namespace Biblioteca.Dominio.ViewModel.AlunoVM
{
    public class AlunoParametroViewModel
    {
        public string? Nome { get; set; }

        public string? Matricula { get; set; }

        public string? DataNascimentoInicio { get; set; }

        public string? DataNascimentoFim { get; set; }

        public int? Sexo { get; set; }

        public bool? SomenteAtivos { get; set; }

        public int? PageIndex { get; set; }

        public int? PageSize { get; set; }

        public string? SortProp { get; set; }

        public string? SortDirection { get; set; }
    }
}
