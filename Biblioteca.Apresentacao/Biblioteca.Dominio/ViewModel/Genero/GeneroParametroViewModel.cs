
namespace Biblioteca.Dominio.ViewModel.Genero
{
    public class GeneroParametroViewModel
    {
        public string? Nome { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public string? SortProp { get; set; }
        public string? SortDirection { get; set; }
    }
}
