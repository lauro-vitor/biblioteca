using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.Livro;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        public async Task ValidarInserirEditar(LivroViewModel livroViewModel)
        {
            if (livroViewModel == null)
                throw new BibliotecaException("Livro inválido");

            if (livroViewModel.Editora == null)
                throw new BibliotecaException("Editora é obrigatória");

            var titulo = livroViewModel.Titulo?.ToLower()?.Trim() ?? string.Empty;

            var existeLivro = await _context.Livro.AnyAsync(l => l.IdLivro != livroViewModel.IdLivro 
                                                              && l.Titulo.ToLower().Trim().Equals(titulo));

            if (existeLivro)
                throw new BibliotecaException("Já existe um livro com este título: " + titulo);

        }
    }
}
