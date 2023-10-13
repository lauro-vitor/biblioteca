using Biblioteca.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Repositorio.EntityFramework
{
	internal class TurmaConfigurationEntityType : IEntityTypeConfiguration<Turma>
	{
		public void Configure(EntityTypeBuilder<Turma> builder)
		{
			builder.HasKey(t => t.IdTurma);

			builder
				.HasOne(turma => turma.Turno)
				.WithMany(turno => turno.Turmas)
				.HasForeignKey(turma => turma.IdTurno);
		}
	}
}
