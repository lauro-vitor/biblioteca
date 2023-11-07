namespace Biblioteca.Dominio.ViewModel
{
    public class LivroViewModel
    {
        public Guid? IdLivro { get; set; }

        public Guid? IdEditora { get; set; }

        public string? Titulo { get; set; }

        public DateOnly? DataPublicacao { get; set; }

        public int? QuantidadeEstoque { get; set; }

        public int? Edicao { get; set; }

        public int? Volume { get; set; }
    }
}
