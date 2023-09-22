using Biblioteca.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Biblioteca.Repositorio.EntityFramework
{
    [ExcludeFromCodeCoverage]
    internal class TurnoEntityTypeConfiguration : IEntityTypeConfiguration<Turno>
    {
        public void Configure(EntityTypeBuilder<Turno> builder)
        {
            builder.HasKey(t => t.IdTurno);
        }
    }
}
