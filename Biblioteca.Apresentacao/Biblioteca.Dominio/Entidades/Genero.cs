using Biblioteca.Dominio.Objetos;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Dominio.Entidades
{
	public class Genero
	{
		private Guid _idGenero;

		[Key]
		public Guid? IdGenero
		{
			get
			{
				return _idGenero;
			}
			set
			{
				if (value == null)
				{
					_idGenero = Guid.NewGuid();
					return;
				}

				if (value == Guid.Empty)
				{
					throw new BibliotecaException("IdGenero: Invalido");
				}

				
				_idGenero = value.Value;
			}
		}

		private string? _nome;
		public string? Nome 
		{
			get
			{
				return _nome;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new BibliotecaException("Nome: Obrigatorio");
				}

				_nome = value.Trim();
			}
		}
	}
}
