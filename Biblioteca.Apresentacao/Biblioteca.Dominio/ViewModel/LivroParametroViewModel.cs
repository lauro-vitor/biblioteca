namespace Biblioteca.Dominio.ViewModel
{
    public class LivroParametroViewModel
    {
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public string? Genero { get; set; }
        public string? Editora { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public string? SortProp { get; set; }
        public string? SortDirection { get; set;}
    }
}
