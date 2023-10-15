using Biblioteca.Dominio.Objetos;

namespace Biblioteca.Dominio.Entidades
{
	public class LivroAutor
	{
		private Guid _idLivro { get; set; }
		public Guid? IdLivro
		{
			get
			{
				return _idLivro;
			}
			set
			{
				if (value == null)
				{
					throw new BibliotecaException("IdLivro: Obrigatorio");
				}

				if (value == Guid.Empty)
				{
					throw new BibliotecaException("IdLivro: Invalido");
				}


				_idLivro = value.Value;
			}
		}

		private Guid _idAutor;
		public Guid? IdAutor
		{
			get
			{
				return _idAutor;
			}
			set
			{
				if (value == null)
				{
					throw new BibliotecaException("IdAutor: Obrigatorio");
				}

				if (value == Guid.Empty)
				{
					throw new BibliotecaException("IdAutor: Invalido");
				}

				_idAutor = value.Value;
			}
		}
	}
}
