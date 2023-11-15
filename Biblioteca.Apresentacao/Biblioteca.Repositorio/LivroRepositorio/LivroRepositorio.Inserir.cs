using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.ViewModel.Livro;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        public async Task<LivroViewModel> Inserir(LivroViewModel livroViewModel)
        {
            await ValidarInserirEditar(livroViewModel);

            var livro = new Livro();

            var editora = await _editoraRepositorio.ObterEditoraPorId(livroViewModel?.Editora?.IdEditora);

            livro.AtribuirLivro(livroViewModel, editora);

            await _context.Livro.AddAsync(livro);

            var livroAutores = await InserirAutor(livro, livroViewModel?.Autores);
            var livroGeneros = await InserirGenero(livro, livroViewModel?.Generos);

            await _context.SaveChangesAsync();

            return livro.ConverterParaLivroViewModel(livroAutores, livroGeneros);
        }
    }
}
