using Biblioteca.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Repositorio.EntityFramework
{
    public class LivroGeneroEntityTypeConfiguration : IEntityTypeConfiguration<LivroGenero>
    {
        public void Configure(EntityTypeBuilder<LivroGenero> builder)
        {
            builder.HasKey(livroGenero => livroGenero.IdLivroGenero);

            builder
                .HasOne(livroGenero => livroGenero.Livro)
                .WithMany(livro => livro.LivroGeneros)
                .HasForeignKey(livroGenero => livroGenero.IdLivro);

            builder
                .HasOne(livroGenero => livroGenero.Genero)
                .WithMany(genero => genero.LivroGeneros)
                .HasForeignKey(livroGenero => livroGenero.IdGenero);
        }
    }
}
