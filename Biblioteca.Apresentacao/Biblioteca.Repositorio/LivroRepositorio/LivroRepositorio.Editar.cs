using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        public async Task<LivroViewModel> Editar(LivroViewModel livroViewModel)
        {
            if (livroViewModel == null)
                throw new BibliotecaException("Livro inválido");

            if (livroViewModel.Editora == null)
                throw new BibliotecaException("Editora Inválida");

            var livro = await _context.Livro
                .Include(l => l.Editora)
                .Include(l => l.LivroAutores)
                .FirstOrDefaultAsync(l => l.IdLivro == livroViewModel.IdLivro);

            if (livro == null)
                throw new BibliotecaException("Livro não encontrado para editar");

            ExcluirAutor(livro);
            ExcluirGenero(livro);

            var editora = await _editoraRepositorio.ObterEditoraPorId(livroViewModel.Editora.IdEditora);
            var autores = await _autorRepositorio.Obter(livroViewModel.Autores);
            var generos = await _generoRepositorio.Obter(livroViewModel.Generos);

            livro.AtribuirLivro(livroViewModel, editora, autores, generos);

            await InserirAutor(livro);
            await InserirGenero(livro);

            _context.Livro.Update(livro);

            await _context.SaveChangesAsync();

            return livro.ConverterParaLivroViewModel();
        }
    }
}
