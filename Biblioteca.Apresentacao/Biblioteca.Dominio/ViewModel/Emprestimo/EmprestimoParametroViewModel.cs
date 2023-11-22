namespace Biblioteca.Dominio.ViewModel.Emprestimo
{
    public class EmprestimoParametroViewModel
    {
        public string? AlunoNome { get; set; }
        public string? AlunoMatricula { get; set; }
        public string? LivroTitulo { get; set; }
        public string? DataEmprestimoInicio { get; set; }
        public string? DataEmprestimoFim { get; set; }
        public string? DataDevolucaoInicio { get; set; }
        public string? DataDevolucaoFim { get; set; }
        public bool? SomenteEmAbertos { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public string? SortProp { get; set; }
        public string? SortDirection { get; set; }
    }
}
