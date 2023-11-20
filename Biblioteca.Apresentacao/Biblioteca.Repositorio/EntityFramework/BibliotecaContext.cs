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
        public DbSet<LivroAutor> LivroAutor => Set<LivroAutor>();
        public DbSet<LivroGenero> LivroGenero => Set<LivroGenero>();
        public DbSet<Parentesco> Parentesco => Set<Parentesco>();  
        public DbSet<Aluno> Aluno => Set<Aluno>();
        public DbSet<AlunoContato> AlunoContato => Set<AlunoContato>();

        public BibliotecaContext(DbContextOptions<BibliotecaContext> options): base(options)
        {   
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            new TurnoEntityTypeConfiguration().Configure(modelbuilder.Entity<Turno>());
            new TurmaEntityTypeConfiguration().Configure(modelbuilder.Entity<Turma>());
      
            new LivroEntityTypeConfiguration().Configure(modelbuilder.Entity<Livro>());
            new EditoraEntityTypeConfiguration().Configure(modelbuilder.Entity<Editora>());
            new AutorEntityTypeConfiguration().Configure(modelbuilder.Entity<Autor>());
            new LivroAutorEntityTypeConfiguration().Configure(modelbuilder.Entity<LivroAutor>());
            new LivroGeneroEntityTypeConfiguration().Configure(modelbuilder.Entity<LivroGenero>());

            new ParentescoEntityTypeConfiguration().Configure(modelbuilder.Entity<Parentesco>());
            new AlunoEntityTypeConfiguration().Configure(modelbuilder.Entity<Aluno>());
            new AlunoContatoEntityTypeConfiguration().Configure(modelbuilder.Entity<AlunoContato>());
        }
    }
}
