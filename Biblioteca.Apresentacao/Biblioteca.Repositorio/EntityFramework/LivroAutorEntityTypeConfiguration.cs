using Biblioteca.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Repositorio.EntityFramework
{
    internal class LivroAutorEntityTypeConfiguration : IEntityTypeConfiguration<LivroAutor>
    {
        public void Configure(EntityTypeBuilder<LivroAutor> builder)
        {
            builder.HasKey(livroAutor => livroAutor.IdLivroAutor);

            builder
                .HasOne(livroAutor => livroAutor.Livro)
                .WithMany(livro => livro.LivroAutores)
                .HasForeignKey(livroAutor => livroAutor.IdLivro);

            builder
                .HasOne(livroAutor => livroAutor.Autor)
                .WithMany(autor => autor.LivroAutores)
                .HasForeignKey(livroAutor => livroAutor.IdAutor);
        }
    }
}
