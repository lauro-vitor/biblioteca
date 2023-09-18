using Biblioteca.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio.EntityFramework
{
    public class BibliotecaContext : DbContext
    {
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
