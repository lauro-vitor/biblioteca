using Biblioteca.Dominio.Objetos;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Dominio.Entidades
{
	public class Autor
	{
		private Guid _idAutor;

		[Key]
		public Guid? IdAutor
		{
			get 
			{ 
				return _idAutor; 
			}

			set
			{
				if(value == null)
				{
					_idAutor = Guid.NewGuid();
					return;
				}

				if(value == Guid.Empty)
				{
					throw new BibliotecaException("IdAutor: Invalido");
				}

				_idAutor = value.Value;
			}
		}

		private string ?_nome;
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
