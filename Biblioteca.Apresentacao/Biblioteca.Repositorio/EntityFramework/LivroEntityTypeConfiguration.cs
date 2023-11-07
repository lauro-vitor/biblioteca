using Biblioteca.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Repositorio.EntityFramework
{
    internal class LivroEntityTypeConfiguration : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.HasKey(livro => livro.IdLivro);

            builder.HasOne(livro => livro.Editora)
                .WithMany(editora => editora.Livros)
                .HasForeignKey(livro => livro.IdEditora);
        }
    }
}
