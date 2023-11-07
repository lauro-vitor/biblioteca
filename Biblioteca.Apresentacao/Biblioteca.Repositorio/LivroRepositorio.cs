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

        public async Task<IEnumerable<Livro>> Obter()
        {
            var query = _context.Livro
                .AsNoTracking()
                .Include(l => l.Editora);

            return await query.ToListAsync();
        }

        public async Task<Livro?> ObterPorId(Guid id)
        {
            return await _context.Livro
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.IdLivro == id);
        }

        public async Task<Livro> Inserir(LivroViewModel livroViewModel)
        {
            if (livroViewModel == null)
                throw new BibliotecaException("Livro inválido");

            var editora = await _context.Editora.FirstOrDefaultAsync(e => e.IdEditora == livroViewModel.IdEditora);

            if (editora == null)
                throw new BibliotecaException("Editora não encontrada");

            var livroParaInserir = new Livro()
            {
                IdLivro = Guid.NewGuid(),
                IdEditora = livroViewModel.IdEditora,
                DataPublicacao = livroViewModel.DataPublicacao,
                Titulo = livroViewModel.Titulo,
                QuantidadeEstoque = livroViewModel.QuantidadeEstoque,
                Edicao = livroViewModel.Edicao,
                Volume = livroViewModel.Volume,
                Editora = editora,
            };

            _context.Livro.Add(livroParaInserir);

            await _context.SaveChangesAsync();

            return livroParaInserir;
        }

        public async Task<Livro> Editar(LivroViewModel livroViewModel)
        {
            if (livroViewModel == null)
                throw new BibliotecaException("Livro inválido");

            var livroParaEditar = await _context.Livro.FirstOrDefaultAsync(l => l.IdLivro == livroViewModel.IdLivro);

            var editora = await _context.Editora.FirstOrDefaultAsync(e => e.IdEditora == livroViewModel.IdEditora);

            if (livroParaEditar == null)
                throw new BibliotecaException("Livro não encontrado para editar");

            if (editora == null)
                throw new BibliotecaException("Editora não encotrada");

            livroParaEditar.IdLivro = livroViewModel.IdLivro;
            livroParaEditar.DataPublicacao = livroViewModel.DataPublicacao;
            livroParaEditar.Titulo = livroViewModel.Titulo;
            livroParaEditar.QuantidadeEstoque = livroViewModel.QuantidadeEstoque;
            livroParaEditar.Edicao = livroViewModel.Edicao;
            livroParaEditar.Volume = livroViewModel.Volume;
            livroParaEditar.IdEditora = livroViewModel.IdEditora;
            livroParaEditar.Editora = editora;

            _context.Livro.Update(livroParaEditar);

            await _context.SaveChangesAsync();

            return livroParaEditar;
        }

        public async Task Excluir(Guid id)
        {
            var livroParaExcluir = await _context.Livro.FirstOrDefaultAsync(l => l.IdLivro == id);

            if (livroParaExcluir == null)
                throw new BibliotecaException("Livro não encontrado para excluir");

            _context.Livro.Remove(livroParaExcluir);

            await _context.SaveChangesAsync();
        }
    }
}
