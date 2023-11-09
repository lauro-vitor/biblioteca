using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.ViewModel;
using Biblioteca.Repositorio.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public class LivroAutorRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;

        public LivroAutorRepositorio(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task Inserir(Livro livro)
        {
            if (livro == null || livro.LivroAutores == null || !livro.LivroAutores.Any())
                return;

            foreach(var livroAutorItem in livro.LivroAutores)
            {
                await _context.LivroAutor.AddAsync(livroAutorItem);
            }
        }

        public void Excluir(Livro livro)
        {
            if (livro == null || livro.LivroAutores == null || !livro.LivroAutores.Any())
                return;

            foreach (var livroAutorItem in livro.LivroAutores)
            {
                _context.LivroAutor.Remove(livroAutorItem);
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
