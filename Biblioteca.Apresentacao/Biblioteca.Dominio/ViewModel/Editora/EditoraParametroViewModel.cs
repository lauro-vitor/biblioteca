namespace Biblioteca.Dominio.ViewModel.Editora
{
    public class EditoraParametroViewModel
    {
        public string? Nome { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public string? SortProp { get; set; }
        public string? SortDirection { get; set; }
    }
}
