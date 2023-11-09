using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public class LivroRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;

        private readonly AutorRepositorio _autorRepositorio;
        private readonly EditoraRepositorio _editoraRepositorio;
        private readonly LivroAutorRepositorio _livroAutorRepositorio;

        public LivroRepositorio(BibliotecaContext context)
        {
            _context = context;
            _autorRepositorio = new AutorRepositorio(context);
            _editoraRepositorio = new EditoraRepositorio(context);
            _livroAutorRepositorio = new LivroAutorRepositorio(context);
        }

        public async Task<IEnumerable<LivroViewModel>> Obter()
        {
            var query = ObterQueryLivroViewModel();

            return await query.ToListAsync();
        }

        public async Task<LivroViewModel?> ObterPorId(Guid id)
        {
            var query = ObterQueryLivroViewModel();

            return await query.FirstOrDefaultAsync(l => l.IdLivro == id);
        }

        public async Task<LivroViewModel> Inserir(LivroViewModel livroViewModel)
        {
            if (livroViewModel == null)
                throw new BibliotecaException("Livro inválido");

            if (livroViewModel.Editora == null)
                throw new BibliotecaException("Editora Inválida");

            var editora = await _editoraRepositorio.ObterEditoraPorId(livroViewModel.Editora.IdEditora);

            var autores = _autorRepositorio.ObterAutores(livroViewModel.Autores);

            var livro = new Livro();
            livro.AtribuirLivro(livroViewModel, editora, autores);

            await _context.Livro.AddAsync(livro);

            await _livroAutorRepositorio.Inserir(livro);

            await _context.SaveChangesAsync();

            return livro.ConverterParaLivroViewModel();
        }

        public async Task Editar(LivroViewModel livroViewModel)
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

            _livroAutorRepositorio.Excluir(livro);

            var editora = await _editoraRepositorio.ObterEditoraPorId(livroViewModel.Editora.IdEditora);

            var autores = _autorRepositorio.ObterAutores(livroViewModel.Autores);

            livro.AtribuirLivro(livroViewModel, editora, autores);

            await _livroAutorRepositorio.Inserir(livro);

            _context.Livro.Update(livro);

            await _context.SaveChangesAsync();
        }

        public async Task Excluir(Guid id)
        {
            var livroParaExcluir = await _context.Livro.FirstOrDefaultAsync(l => l.IdLivro == id);

            if (livroParaExcluir == null)
                throw new BibliotecaException("Livro não encontrado para excluir");

            _context.Livro.Remove(livroParaExcluir);

            await _context.SaveChangesAsync();
        }


        //private async Task ValidarSalvar(LivroViewModel livroViewModel)
        //{
        //    if (livroViewModel == null)
        //        throw new BibliotecaException("Livro inválido");

        //    if (livroViewModel.Editora == null)
        //        throw new BibliotecaException("Editora Inválida");

        //    var existeLivro = await _context.Livro.AnyAsync(l => l.IdLivro != livroViewModel.IdLivro
        //                                                      && l.Titulo.ToLower().Trim().Equals(livroViewModel.Titulo.ToLower().Trim()));

        //    if (existeLivro)
        //        throw new BibliotecaException("Exise um livro cadastrado com este nome");

        //}

        private IQueryable<LivroViewModel> ObterQueryLivroViewModel()
        {
            #nullable disable
            var query = _context.Livro
                    .Include(l => l.Editora)
                    .Include(l => l.LivroAutores).ThenInclude(l => l.Autor)
                    .AsNoTracking()
                    .Select(l => l.ConverterParaLivroViewModel());

            return query;
        }

        public void Dispose()
        {
            _context?.Dispose();
            _autorRepositorio.Dispose();
            _editoraRepositorio?.Dispose();
            _livroAutorRepositorio?.Dispose();
        }
    }
}
