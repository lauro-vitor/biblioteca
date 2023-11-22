using Biblioteca.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Repositorio.EntityFramework
{
    public class EmprestimoEntityTypeConfiguration : IEntityTypeConfiguration<Emprestimo>
    {
        public void Configure(EntityTypeBuilder<Emprestimo> builder)
        {
            builder.HasKey(e => e.IdEmprestimo);

            builder.HasOne(e => e.Livro)
                .WithMany(l => l.Emprestimos)
                .HasForeignKey(e => e.IdLivro);

            builder.HasOne(e => e.Aluno)
                .WithMany(a => a.Emprestimos)
                .HasForeignKey(e => e.IdAluno);
        }
    }
}
