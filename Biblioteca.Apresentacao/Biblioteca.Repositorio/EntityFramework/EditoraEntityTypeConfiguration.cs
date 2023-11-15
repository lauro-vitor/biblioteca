using Biblioteca.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Repositorio.EntityFramework
{
    internal class EditoraEntityTypeConfiguration : IEntityTypeConfiguration<Editora>
    {
        public void Configure(EntityTypeBuilder<Editora> builder)
        {
            builder.HasKey(e => e.IdEditora);
        }
    }
}
