using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public class LivroRepositorio
    {
        private readonly BibliotecaContext _context;

        public LivroRepositorio(BibliotecaContext context)
        {
            _context = context;
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

        public async Task<Guid> Inserir(LivroViewModel livroViewModel)
        {
            if (livroViewModel == null)
                throw new BibliotecaException("Livro inválido");

            if (livroViewModel.Editora == null)
                throw new BibliotecaException("Editora Inválida");

            var livroParaInserir = new Livro()
            {
                IdLivro = Guid.NewGuid(),
                IdEditora = livroViewModel.Editora.IdEditora,
                DataPublicacao = livroViewModel.DataPublicacao,
                Titulo = livroViewModel.Titulo,
                QuantidadeEstoque = livroViewModel.QuantidadeEstoque,
                Edicao = livroViewModel.Edicao,
                Volume = livroViewModel.Volume,
            };

            await _context.Livro.AddAsync(livroParaInserir);

            await VincularLivroAutor(livroParaInserir, livroViewModel.Autores);

            await _context.SaveChangesAsync();

            return livroParaInserir.IdLivro.Value;
        }

        public async Task Editar(LivroViewModel livroViewModel)
        {
            if (livroViewModel == null)
                throw new BibliotecaException("Livro inválido");

            if (livroViewModel.Editora == null)
                throw new BibliotecaException("Editora Inválida");

            var livroParaEditar = await _context.Livro
                .Include(l => l.Editora)
                .Include(l => l.LivroAutores)
                .FirstOrDefaultAsync(l => l.IdLivro == livroViewModel.IdLivro);

            if (livroParaEditar == null)
                throw new BibliotecaException("Livro não encontrado para editar");

            livroParaEditar.IdLivro = livroViewModel.IdLivro;
            livroParaEditar.DataPublicacao = livroViewModel.DataPublicacao;
            livroParaEditar.Titulo = livroViewModel.Titulo;
            livroParaEditar.QuantidadeEstoque = livroViewModel.QuantidadeEstoque;
            livroParaEditar.Edicao = livroViewModel.Edicao;
            livroParaEditar.Volume = livroViewModel.Volume;
            livroParaEditar.IdEditora = livroViewModel.Editora.IdEditora;

            DesvincularLivroAutor(livroParaEditar);

            await VincularLivroAutor(livroParaEditar, livroViewModel.Autores);

            _context.Livro.Update(livroParaEditar);

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

        private async Task VincularLivroAutor(Livro livro, IEnumerable<AutorViewModel>? autoresViewModel)
        {
            if (autoresViewModel == null || !autoresViewModel.Any())
                return;

            var idsAutores = autoresViewModel.Select(a => a.IdAutor).ToList();

            var autores = _context.Autor.Where(a => idsAutores.Contains(a.IdAutor));

            foreach (var autor in autores)
            {
                var livroAutor = new LivroAutor()
                {
                    IdLivroAutor = Guid.NewGuid(),
                    Livro = livro,
                    Autor = autor,
                };

                await _context.LivroAutor.AddAsync(livroAutor);
            }
        }

        private void DesvincularLivroAutor(Livro livro)
        {
            if (livro.LivroAutores == null || !livro.LivroAutores.Any())
                return;

            foreach (var livroAutorItem in livro.LivroAutores)
            {
                _context.LivroAutor.Remove(livroAutorItem);
            }
        }

        private IQueryable<LivroViewModel> ObterQueryLivroViewModel()
        {
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
                        })
                    });

            return query;
        }
    }
}
