using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        public async Task<LivroViewModel> Editar(LivroViewModel livroViewModel)
        {
            await ValidarInserirEditar(livroViewModel);

            var livro = await _context.Livro
                .Include(l => l.Editora)
                .FirstOrDefaultAsync(l => l.IdLivro == livroViewModel.IdLivro);

            if (livro == null)
                throw new BibliotecaException("Livro não encontrado para editar");

            var editora = await _editoraRepositorio.ObterEditoraPorId(livroViewModel?.Editora?.IdEditora);

            livro.AtribuirLivro(livroViewModel, editora);

            await ExcluirAutor(livro.IdLivro);
            await ExcluirGenero(livro.IdLivro);

            var livroAutores = await InserirAutor(livro, livroViewModel?.Autores);
            var livroGeneros = await InserirGenero(livro, livroViewModel?.Generos);

            await _context.SaveChangesAsync();

            return livro.ConverterParaLivroViewModel(livroAutores, livroGeneros);
        }
    }
}
