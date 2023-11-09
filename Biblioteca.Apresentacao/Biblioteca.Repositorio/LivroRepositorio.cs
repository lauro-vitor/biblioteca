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

            _livroAutorRepositorio.Excluir(livro);

            var editora = await _editoraRepositorio.ObterEditoraPorId(livroViewModel.Editora.IdEditora);

            var autores = _autorRepositorio.ObterAutores(livroViewModel.Autores);

            livro.AtribuirLivro(livroViewModel, editora, autores);

            await _livroAutorRepositorio.Inserir(livro);

            _context.Livro.Update(livro);

            await _context.SaveChangesAsync();

            return livro.ConverterParaLivroViewModel();
        }

        public async Task Excluir(Guid id)
        {
            var livroParaExcluir = await _context.Livro.FirstOrDefaultAsync(l => l.IdLivro == id);

            if (livroParaExcluir == null)
                throw new BibliotecaException("Livro não encontrado para excluir");

            _context.Livro.Remove(livroParaExcluir);

            await _context.SaveChangesAsync();
        }

        private IQueryable<LivroViewModel> ObterQueryLivroViewModel()
        {
            #nullable disable
            var query = _context.Livro
                    .AsNoTracking()
                    .Select(l => new LivroViewModel
                    {
                        IdLivro = l.IdLivro,
                        Titulo = l.Titulo,
                        DataPublicacao = l.DataPublicacao,
                        QuantidadeEstoque = l.QuantidadeEstoque,
                        Edicao = l.Edicao,
                        Volume = l.Volume,
                        Editora = new EditoraViewModel
                        {
                            IdEditora = l.IdEditora,
                            Nome = l.Editora.Nome ?? "",
                        },
                        Autores = l.LivroAutores.Select(la => new AutorViewModel
                        {
                            IdAutor = la.IdAutor,
                            Nome = la.Autor.Nome,
                        }).ToList()
                    });

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
