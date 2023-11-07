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
            var livroParaInserir = new Livro()
            {
                IdLivro = Guid.NewGuid(),
                IdEditora = livroViewModel.IdEditora,
                DataPublicacao = livroViewModel.DataPublicacao,
                Titulo = livroViewModel.Titulo,
                QuantidadeEstoque = livroViewModel.QuantidadeEstoque,
                Edicao = livroViewModel.Edicao,
                Volume = livroViewModel.Volume,
            };

            _context.Livro.Add(livroParaInserir);

            await _context.SaveChangesAsync();

            return livroParaInserir;
        }

        public async Task<Livro> Editar(LivroViewModel livroViewModel)
        {
            var livroParaEditar = await _context.Livro.FirstOrDefaultAsync(l => l.IdLivro == livroViewModel.IdLivro);

            if (livroParaEditar == null)
                throw new BibliotecaException("Livro não encontrado para editar");

            livroParaEditar.IdLivro = livroViewModel.IdLivro;
            livroParaEditar.IdEditora = livroViewModel.IdEditora;
            livroParaEditar.DataPublicacao = livroViewModel.DataPublicacao;
            livroParaEditar.Titulo = livroViewModel.Titulo;
            livroParaEditar.QuantidadeEstoque = livroViewModel.QuantidadeEstoque;
            livroParaEditar.Edicao = livroViewModel.Edicao;
            livroParaEditar.Volume = livroViewModel.Volume;


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
