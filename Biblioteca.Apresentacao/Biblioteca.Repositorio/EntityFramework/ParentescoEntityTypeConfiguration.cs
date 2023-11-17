using Biblioteca.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Repositorio.EntityFramework
{
    internal class ParentescoEntityTypeConfiguration : IEntityTypeConfiguration<Parentesco>
    {
        public void Configure(EntityTypeBuilder<Parentesco> builder)
        {
            builder.HasKey(p => p.IdParentesco);
        }
    }
}
