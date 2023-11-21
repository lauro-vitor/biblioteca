using Biblioteca.Dominio.ViewModel.Editora;

namespace Biblioteca.Dominio.ViewModel.Livro
{
    public class LivroInputViewModel
    {
        public Guid? IdLivro { get; set; }

        public string? Titulo { get; set; }

        public DateOnly? DataPublicacao { get; set; }

        public int? QuantidadeEstoque { get; set; }

        public int? Edicao { get; set; }

        public int? Volume { get; set; }

        public EditoraViewModel? Editora { get; set; }
    }
}
