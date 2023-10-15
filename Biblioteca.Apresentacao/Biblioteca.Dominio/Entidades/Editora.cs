using Biblioteca.Dominio.Objetos;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Dominio.Entidades
{
	public class Editora
	{
		private Guid _idEditora;

		[Key]
		public Guid? IdEditora
		{
			get
			{
				return _idEditora;
			}
			set
			{
				if(value == null)
				{
					_idEditora = Guid.NewGuid();
					return;
				}

				if(value == Guid.Empty) 
				{
					throw new BibliotecaException("IdEditora: Invalido");
				}

				_idEditora = value.Value;
			}
		}

		private string _nome;

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
