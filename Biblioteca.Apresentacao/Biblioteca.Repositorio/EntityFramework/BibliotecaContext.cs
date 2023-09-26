using Biblioteca.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Biblioteca.Repositorio.EntityFramework
{
    [ExcludeFromCodeCoverage]
    public class BibliotecaContext : DbContext
    {
        public DbSet<Turma> Turma => Set<Turma>();
        public DbSet<Turno> Turno => Set<Turno>();

        public BibliotecaContext(DbContextOptions<BibliotecaContext> options): base(options)
        {   
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            new TurnoEntityTypeConfiguration().Configure(modelbuilder.Entity<Turno>());
        }
    }
}
