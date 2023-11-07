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
        public DbSet<Editora> Editora => Set<Editora>();
        public DbSet<Autor> Autor => Set<Autor>();
        public DbSet<Genero> Genero => Set<Genero>();
        public DbSet<Livro> Livro => Set<Livro>();

        public BibliotecaContext(DbContextOptions<BibliotecaContext> options): base(options)
        {   
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            new TurnoEntityTypeConfiguration().Configure(modelbuilder.Entity<Turno>());
            new TurmaEntityTypeConfiguration().Configure(modelbuilder.Entity<Turma>());
            new LivroEntityTypeConfiguration().Configure(modelbuilder.Entity<Livro>());
        }
    }
}
