using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        public async Task<LivroViewModel> Inserir(LivroViewModel livroViewModel)
        {
            if (livroViewModel == null)
                throw new BibliotecaException("Livro inválido");

            if (livroViewModel.Editora == null)
                throw new BibliotecaException("Editora Inválida");

            var editora = await _editoraRepositorio.ObterEditoraPorId(livroViewModel.Editora.IdEditora);
            var autores = await _autorRepositorio.Obter(livroViewModel.Autores);
            var generos = await _generoRepositorio.Obter(livroViewModel.Generos);

            var livro = new Livro();

            livro.AtribuirLivro(livroViewModel, editora, autores, generos);

            await _context.Livro.AddAsync(livro);

            await InserirAutor(livro);
            await InserirGenero(livro);

            await _context.SaveChangesAsync();

            return livro.ConverterParaLivroViewModel();
        }
    }
}
