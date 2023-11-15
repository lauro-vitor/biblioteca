using Biblioteca.Dominio.ViewModel.Autor;
using Biblioteca.Dominio.ViewModel.Genero;

namespace Biblioteca.Dominio.ViewModel.Livro
{
    public class LivroViewModel
    {
        public Guid? IdLivro { get; set; }

        public string? Titulo { get; set; }

        public DateOnly? DataPublicacao { get; set; }

        public int? QuantidadeEstoque { get; set; }

        public int? Edicao { get; set; }

        public int? Volume { get; set; }

        public EditoraViewModel? Editora { get; set; }

        public ICollection<AutorViewModel>? Autores { get; set; }

        public ICollection<GeneroViewModel>? Generos { get; set; }
    }
}
