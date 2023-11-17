using Biblioteca.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Repositorio.EntityFramework
{
    internal class AlunoContatoEntityTypeConfiguration : IEntityTypeConfiguration<AlunoContato>
    {
        public void Configure(EntityTypeBuilder<AlunoContato> builder)
        {
            builder.HasKey(ac => ac.IdContato);

            builder.HasOne(alunoContato => alunoContato.Aluno)
                .WithMany(aluno => aluno.AlunoContatos)
                .HasForeignKey(alunoContato => alunoContato.IdAluno);


            builder.HasOne(alunoContato => alunoContato.Parentesco)
                .WithMany(parentesco => parentesco.AlunoContatos)
                .HasForeignKey(alunoContato => alunoContato.IdParentesco);
        }
    }
}
