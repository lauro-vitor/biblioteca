using Biblioteca.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Repositorio.EntityFramework
{
    internal class LivroEntityTypeConfiguration : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder
                .HasOne(l => l.Editora)
                .WithMany(e => e.Livros)
                .HasForeignKey(l => l.IdEditora);
        }
    }
}
